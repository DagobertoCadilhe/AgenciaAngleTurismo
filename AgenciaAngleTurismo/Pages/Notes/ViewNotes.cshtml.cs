using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;

namespace AgenciaTurismo.Pages.Notes
{
    public class ViewNotesModel : PageModel
    {
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<ViewNotesModel> _logger;

        public ViewNotesModel(IWebHostEnvironment env, ILogger<ViewNotesModel> logger)
        {
            _env = env;
            _logger = logger;
        }

        [BindProperty]
        [Required(ErrorMessage = "O conteúdo da nota é obrigatório")]
        [StringLength(1000, ErrorMessage = "A nota não pode exceder 1000 caracteres")]
        public string NoteContent { get; set; }

        public List<string> AvailableNotes { get; set; } = new List<string>();
        public string CurrentNoteContent { get; set; }
        public string CurrentNoteName { get; set; }

        public void OnGet(string noteName = null)
        {
            try
            {
                LoadAvailableNotes();

                if (!string.IsNullOrEmpty(noteName))
                {
                    LoadNoteContent(noteName);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao carregar notas");
                TempData["ErrorMessage"] = "Ocorreu um erro ao carregar as notas";
            }
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                LoadAvailableNotes();
                return Page();
            }

            try
            {
                var fileName = $"note_{DateTime.Now:yyyyMMddHHmmss}.txt";
                var filePath = GetSafeFilePath(fileName);

                Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                System.IO.File.WriteAllText(filePath, NoteContent);

                TempData["SuccessMessage"] = "Nota salva com sucesso!";
                return RedirectToPage(new { noteName = (string)null });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao salvar nota");
                TempData["ErrorMessage"] = "Ocorreu um erro ao salvar a nota";
                LoadAvailableNotes();
                return Page();
            }
        }

        public IActionResult OnPostDelete(string noteName)
        {
            try
            {
                var filePath = GetSafeFilePath(noteName);

                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                    TempData["SuccessMessage"] = "Nota excluída com sucesso!";
                }

                return RedirectToPage(new { noteName = (string)null });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao excluir nota");
                TempData["ErrorMessage"] = "Ocorreu um erro ao excluir a nota";
                return RedirectToPage();
            }
        }

        private void LoadAvailableNotes()
        {
            var filesDir = Path.Combine(_env.WebRootPath, "files");

            if (Directory.Exists(filesDir))
            {
                AvailableNotes = Directory.GetFiles(filesDir, "*.txt")
                    .OrderByDescending(f => new FileInfo(f).CreationTime)
                    .Select(Path.GetFileName)
                    .ToList();
            }
        }

        private void LoadNoteContent(string noteName)
        {
            var safeFileName = Path.GetFileName(noteName);
            var filePath = GetSafeFilePath(safeFileName);

            if (System.IO.File.Exists(filePath))
            {
                CurrentNoteContent = System.IO.File.ReadAllText(filePath);
                CurrentNoteName = safeFileName;
            }
        }

        private string GetSafeFilePath(string fileName)
        {
            var safeFileName = Path.GetFileName(fileName);
            return Path.Combine(_env.WebRootPath, "files", safeFileName);
        }
    }
}