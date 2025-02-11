using GestionAsistencia.Data;
using GestionAsistencia.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Text;
using Twilio;
using Twilio.Exceptions;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace GestionAsistencia.Controllers
{
    [Authorize(Roles = "Profesor")]
    public class ProfesorController : Controller
    {
        private readonly AppDbContext _context;

        public ProfesorController(AppDbContext context)
        {
            _context = context;
        }

        // Acción para mostrar los cursos del día actual
        public IActionResult CursosDelDia()
        {

            var profesorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(profesorId))
            {
                Console.WriteLine("No se pudo obtener el ID del profesor.");
                return Content("No se pudo obtener el ID del profesor.");
            }

            int profesorIdInt = int.Parse(profesorId);

            var diaSemana = DateTime.Now.DayOfWeek switch
            {
                DayOfWeek.Monday => "Lunes",
                DayOfWeek.Tuesday => "Martes",
                DayOfWeek.Wednesday => "Miércoles",
                DayOfWeek.Thursday => "Jueves",
                DayOfWeek.Friday => "Viernes",
                DayOfWeek.Saturday => "Sábado",
                DayOfWeek.Sunday => "Domingo",
                _ => ""
            };

            Console.WriteLine($"ProfesorId: {profesorIdInt}, DiaSemana: {diaSemana}");

            var horarios = _context.Horario
                .Where(h => h.ProfesorId == profesorIdInt && h.DiaSemana == diaSemana)
                .Include(h => h.Grado)
                .Include(h => h.Materia)
                .ToList();

            Console.WriteLine($"Cantidad de horarios encontrados: {horarios.Count}");

            if (!horarios.Any())
            {
                ViewBag.Mensaje = "No tienes clases asignadas para el día de hoy.";
            }

            return View(horarios);
        }

        // Acción para mostrar el listado de estudiantes de un curso específico
        public IActionResult ListaEstudiantes(int horarioId)
        {
            var estudiantes = _context.Estudiantes
                .Where(e => e.GradoId == _context.Horario.First(h => h.Id == horarioId).GradoId)
                .ToList();

            var fechaHoy = DateTime.Now.Date;
            var asistencias = _context.Asistencias
                .Where(a => a.HorarioId == horarioId && a.Fecha == fechaHoy)
                .ToList();

            ViewBag.Asistencias = asistencias;
            ViewBag.HorarioId = horarioId;
            return View(estudiantes);
        }

        [HttpPost]
        public async Task<IActionResult> GuardarAsistencia(int horarioId, IFormCollection form)
        {

            try
            {
                var estudiantesEnHorario = _context.Estudiantes
                .Where(e => e.GradoId == _context.Horario.First(h => h.Id == horarioId).GradoId)
                .ToList();

                var fechaHoy = DateTime.Now.Date;

                var whatsAppService = new WhatsAppService(); // Instancia del servicio

                foreach (var estudiante in estudiantesEnHorario)
                {
                    var estadoStr = form[$"estado_{estudiante.Id}"]; // Obtén el estado seleccionado

                    if (!Enum.TryParse(estadoStr, out EstadoAsistencia estado))
                    {
                        estado = EstadoAsistencia.Asistio; // Si no se recibe dato, se asume asistencia
                    }

                    // Buscar si ya existe una asistencia para este estudiante en esta fecha
                    var asistenciaExistente = _context.Asistencias
                        .FirstOrDefault(a => a.EstudianteId == estudiante.Id && a.HorarioId == horarioId && a.Fecha == fechaHoy);

                    if (asistenciaExistente != null)
                    {
                        // Guardar el estado anterior
                        var estadoAnterior = asistenciaExistente.Estado;

                        // Si ya existe, actualizar el estado
                        asistenciaExistente.Estado = estado;
                        _context.Asistencias.Update(asistenciaExistente);

                        // Solo enviar el mensaje si el estado cambió a "Inasistencia" y antes no lo era
                        if (estado == EstadoAsistencia.Inasistencia && estadoAnterior != EstadoAsistencia.Inasistencia)
                        {
                            await EnviarMensajeInasistencia(estudiante, horarioId);
                        }

                    }
                    else
                    {
                        var asistencia = new Asistencia
                        {
                            EstudianteId = estudiante.Id,
                            HorarioId = horarioId,
                            Fecha = fechaHoy,
                            Estado = estado
                        };

                        _context.Asistencias.Add(asistencia);

                        // Enviar mensaje solo si el nuevo estado es "Inasistencia"
                        if (estado == EstadoAsistencia.Inasistencia)
                        {
                            await EnviarMensajeInasistencia(estudiante, horarioId);
                        }
                    }

                }

                _context.SaveChanges();
                return RedirectToAction("CursosDelDia");
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = $"Error general: {ex.Message}";
                return View("ListaEstudiantes", _context.Estudiantes
                    .Where(e => e.GradoId == _context.Horario.First(h => h.Id == horarioId).GradoId)
                    .ToList());
            }

        }

        private async Task EnviarMensajeInasistencia(Estudiante estudiante, int horarioId)
        {
            var horario = _context.Horario
                .Include(h => h.Grado)
                .Include(h => h.Materia)
                .FirstOrDefault(h => h.Id == horarioId);

            string numeroPadre = "+57" + estudiante.ContactoPadres;

            if (!string.IsNullOrEmpty(numeroPadre))
            {
                string mensaje = $"Estimado padre/madre, le informamos que su hijo {estudiante.Nombre} no asistió hoy a la clase de {horario.Materia.Nombre} del grado {horario.Grado.Nombre}.";

                try
                {
                    var whatsAppService = new WhatsAppService();
                    await whatsAppService.EnviarMensajeWhatsApp(numeroPadre, mensaje);
                }
                catch (ApiException ex)
                {
                    ViewBag.Mensaje = $"Error al enviar WhatsApp: {ex.Message} (Código: {ex.Code})";
                    ViewBag.EsError = true;
                }
                catch (Exception ex)
                {
                    ViewBag.Mensaje = $"Error inesperado: {ex.Message}";
                    ViewBag.EsError = true;
                }
            }
        }


        [HttpGet]
        public IActionResult Horario()
        {
            // Obtén el ID del profesor autenticado
            var profesorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            // Carga los horarios agrupados por día de la semana
            var horarios = _context.Horario
                .Where(h => h.ProfesorId == profesorId)
                .Include(h => h.Materia)
                .Include(h => h.Grado)
                .GroupBy(h => h.DiaSemana)
                .ToList();

            return View(horarios); // Asegúrate de que la vista exista
        }

    }



    public class WhatsAppService
    {
        public async Task EnviarMensajeWhatsApp(string numeroDestino, string mensaje)
        {
            string accountSid = "ACe99fbdb5d1a18234cba779ff965b0421";
            string authToken = "6099bae94cd719cf2c2cd7c93b7cf3c9";

            TwilioClient.Init(accountSid, authToken);

            var message = MessageResource.Create(
            from: new PhoneNumber("whatsapp:+14155238886"),  // Twilio Sandbox
            to: new PhoneNumber("whatsapp:" + numeroDestino),
            body: mensaje);

        }
    }

}
