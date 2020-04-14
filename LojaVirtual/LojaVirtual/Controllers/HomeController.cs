﻿using LojaVirtual.Database;
using LojaVirtual.Libraries.Email;
using LojaVirtual.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LojaVirtual.Controllers
{
    public class HomeController : Controller
    {
        private LojaVirtualContext _banco;
        public HomeController(LojaVirtualContext banco)
        {
            _banco = banco;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index([FromForm]NewsletterEmail newsletter)
        {
            if (ModelState.IsValid)
            {
                _banco.NewsletterEmail.Add(newsletter);
                _banco.SaveChanges();

                TempData["MSG_S"] = "E-mail cadastrado! Agora você irá receber promoções especiais no seu e-mail! Fique atento as novidades!";

                return RedirectToAction(nameof(Index));
            }
            else
                return View();
        }

        public IActionResult Contato()
        {
            return View();
        }

        public IActionResult ContatoAcao()
        {
            try
            {
                Contato contato = new Contato()
                {
                    Nome = HttpContext.Request.Form["nome"],
                    Email = HttpContext.Request.Form["email"],
                    Texto = HttpContext.Request.Form["texto"]
                };

                List<ValidationResult> listaMensagens = new List<ValidationResult>();
                ValidationContext contexto = new ValidationContext(contato);

                if (Validator.TryValidateObject(contato, contexto, listaMensagens, true))
                {
                    ContatoEmail.EnviarContatoPorEmail(contato);
                    ViewData["MSG_S"] = "Mensagem de contato enviada com sucesso!";
                }
                else
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    foreach (ValidationResult validationResult in listaMensagens)
                    {
                        stringBuilder.Append(validationResult.ErrorMessage);
                        stringBuilder.Append("<br/>");
                        stringBuilder.Append("<br/>");
                    }

                    ViewData["MSG_E"] = stringBuilder.ToString();
                    ViewData["CONTATO"] = contato;
                }
            }
            catch(Exception)
            {
                ViewData["MSG_E"] = "Oops! Tivemos um erro, texte novamente mais terde!";
                //TODO - Implementar log
            }
            return View("Contato");
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult CadastroCliente()
        {
            return View();
        }

        public IActionResult CarrinhoCompras()
        {
            return View();
        }
    }
}