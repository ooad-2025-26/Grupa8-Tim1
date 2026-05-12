using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DigitalniKlubCitalaca.Data;
using DigitalniKlubCitalaca.Models;

namespace DigitalniKlubCitalaca.Controllers
{
    public class CitalackaGrupaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CitalackaGrupaController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CitalackaGrupa
        public async Task<IActionResult> Index()
        {
            return View(await _context.CitalackeGrupe.ToListAsync());
        }

        // GET: CitalackaGrupa/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var citalackaGrupa = await _context.CitalackeGrupe
                .FirstOrDefaultAsync(m => m.GrupaId == id);
            if (citalackaGrupa == null)
            {
                return NotFound();
            }

            return View(citalackaGrupa);
        }

        // GET: CitalackaGrupa/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CitalackaGrupa/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GrupaId,Naziv,Opis,Zanr,Regija,Grad,MaksimalanBrojClanova,TipGrupe,DatumKreiranja")] CitalackaGrupa citalackaGrupa)
        {
            if (ModelState.IsValid)
            {
                _context.Add(citalackaGrupa);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(citalackaGrupa);
        }

        // GET: CitalackaGrupa/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var citalackaGrupa = await _context.CitalackeGrupe.FindAsync(id);
            if (citalackaGrupa == null)
            {
                return NotFound();
            }
            return View(citalackaGrupa);
        }

        // POST: CitalackaGrupa/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("GrupaId,Naziv,Opis,Zanr,Regija,Grad,MaksimalanBrojClanova,TipGrupe,DatumKreiranja")] CitalackaGrupa citalackaGrupa)
        {
            if (id != citalackaGrupa.GrupaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(citalackaGrupa);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CitalackaGrupaExists(citalackaGrupa.GrupaId))
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
            return View(citalackaGrupa);
        }

        // GET: CitalackaGrupa/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var citalackaGrupa = await _context.CitalackeGrupe
                .FirstOrDefaultAsync(m => m.GrupaId == id);
            if (citalackaGrupa == null)
            {
                return NotFound();
            }

            return View(citalackaGrupa);
        }

        // POST: CitalackaGrupa/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var citalackaGrupa = await _context.CitalackeGrupe.FindAsync(id);
            if (citalackaGrupa != null)
            {
                _context.CitalackeGrupe.Remove(citalackaGrupa);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CitalackaGrupaExists(int id)
        {
            return _context.CitalackeGrupe.Any(e => e.GrupaId == id);
        }
    }
}
