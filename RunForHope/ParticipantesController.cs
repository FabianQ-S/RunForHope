using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RunForHope.Models;
using RunForHope_️.Data;

namespace RunForHope_️
{
    public class ParticipantesController : Controller
    {
        private readonly ParticipantesContext _context;

        public ParticipantesController(ParticipantesContext context)
        {
            _context = context;
        }

        // GET: Participantes
        public async Task<IActionResult> Index()
        {
            return View(await _context.Participantes.ToListAsync());
        }

        // GET: Participantes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var participantes = await _context.Participantes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (participantes == null)
            {
                return NotFound();
            }

            return View(participantes);
        }

        // GET: Participantes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Participantes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Apellido,DNI,FechaNacimiento,Genero,Discapacidad,Correo,NumeroTelefono,Distrito,TipoSangre")] Participantes participantes)
        {
            if (ModelState.IsValid)
            {
                _context.Add(participantes);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(participantes);
        }

        // GET: Participantes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var participantes = await _context.Participantes.FindAsync(id);
            if (participantes == null)
            {
                return NotFound();
            }
            return View(participantes);
        }

        // POST: Participantes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Apellido,DNI,FechaNacimiento,Genero,Discapacidad,Correo,NumeroTelefono,Distrito,TipoSangre")] Participantes participantes)
        {
            if (id != participantes.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(participantes);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ParticipantesExists(participantes.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(participantes);
        }

        // GET: Participantes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var participantes = await _context.Participantes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (participantes == null)
            {
                return NotFound();
            }

            return View(participantes);
        }

        // POST: Participantes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var participantes = await _context.Participantes.FindAsync(id);
            if (participantes != null)
            {
                _context.Participantes.Remove(participantes);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ParticipantesExists(int id)
        {
            return _context.Participantes.Any(e => e.Id == id);
        }
    }
}
