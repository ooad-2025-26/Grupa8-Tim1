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
    public class SadrzajGrupeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SadrzajGrupeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: SadrzajGrupe
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.SadrzajiGrupe.Include(s => s.Autor).Include(s => s.CitalackaGrupa);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: SadrzajGrupe/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sadrzajGrupe = await _context.SadrzajiGrupe
                .Include(s => s.Autor)
                .Include(s => s.CitalackaGrupa)
                .FirstOrDefaultAsync(m => m.SadrzajId == id);
            if (sadrzajGrupe == null)
            {
                return NotFound();
            }

            return View(sadrzajGrupe);
        }

        // GET: SadrzajGrupe/Create
        public IActionResult Create()
        {
            ViewData["AutorId"] = new SelectList(_context.Korisnici, "KorisnikId", "KorisnikId");
            ViewData["GrupaId"] = new SelectList(_context.CitalackeGrupe, "GrupaId", "GrupaId");
            return View();
        }

        // POST: SadrzajGrupe/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SadrzajId,AutorId,GrupaId,DatumObjave,Naslov,Opis,Link,TipSadrzaja,StatusSadrzaja")] SadrzajGrupe sadrzajGrupe)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sadrzajGrupe);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AutorId"] = new SelectList(_context.Korisnici, "KorisnikId", "KorisnikId", sadrzajGrupe.AutorId);
            ViewData["GrupaId"] = new SelectList(_context.CitalackeGrupe, "GrupaId", "GrupaId", sadrzajGrupe.GrupaId);
            return View(sadrzajGrupe);
        }

        // GET: SadrzajGrupe/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sadrzajGrupe = await _context.SadrzajiGrupe.FindAsync(id);
            if (sadrzajGrupe == null)
            {
                return NotFound();
            }
            ViewData["AutorId"] = new SelectList(_context.Korisnici, "KorisnikId", "KorisnikId", sadrzajGrupe.AutorId);
            ViewData["GrupaId"] = new SelectList(_context.CitalackeGrupe, "GrupaId", "GrupaId", sadrzajGrupe.GrupaId);
            return View(sadrzajGrupe);
        }

        // POST: SadrzajGrupe/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SadrzajId,AutorId,GrupaId,DatumObjave,Naslov,Opis,Link,TipSadrzaja,StatusSadrzaja")] SadrzajGrupe sadrzajGrupe)
        {
            if (id != sadrzajGrupe.SadrzajId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sadrzajGrupe);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SadrzajGrupeExists(sadrzajGrupe.SadrzajId))
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
            ViewData["AutorId"] = new SelectList(_context.Korisnici, "KorisnikId", "KorisnikId", sadrzajGrupe.AutorId);
            ViewData["GrupaId"] = new SelectList(_context.CitalackeGrupe, "GrupaId", "GrupaId", sadrzajGrupe.GrupaId);
            return View(sadrzajGrupe);
        }

        // GET: SadrzajGrupe/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sadrzajGrupe = await _context.SadrzajiGrupe
                .Include(s => s.Autor)
                .Include(s => s.CitalackaGrupa)
                .FirstOrDefaultAsync(m => m.SadrzajId == id);
            if (sadrzajGrupe == null)
            {
                return NotFound();
            }

            return View(sadrzajGrupe);
        }

        // POST: SadrzajGrupe/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sadrzajGrupe = await _context.SadrzajiGrupe.FindAsync(id);
            if (sadrzajGrupe != null)
            {
                _context.SadrzajiGrupe.Remove(sadrzajGrupe);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SadrzajGrupeExists(int id)
        {
            return _context.SadrzajiGrupe.Any(e => e.SadrzajId == id);
        }
    }
}
