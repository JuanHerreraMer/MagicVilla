﻿using AutoMapper;
using MagicVilla_Utilidad;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Models.ViewModels;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace MagicVilla_Web.Controllers
{
    public class NumeroVillaController : Controller
    {
        private readonly INumeroVillaService _numeroVillaService;
        private readonly IVillaService _villaService;
        private readonly IMapper _mapper;
        public NumeroVillaController(IVillaService villaService, INumeroVillaService numeroVillaService, IMapper mapper)
        {
            _villaService = villaService;
            _numeroVillaService = numeroVillaService;
            _mapper = mapper;
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> IndexNumeroVilla()
        {
            List<NumeroVillaDto> numeroVillaList = new();

            var response = await _numeroVillaService.ObtenerTodos<APIResponse>(HttpContext.Session.GetString(DS.SessionToken));
            if (response != null && response.isExitoso)
            {
                numeroVillaList = JsonConvert.DeserializeObject<List<NumeroVillaDto>>(Convert.ToString(response.Resultado));
            }

            return View(numeroVillaList);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> CrearNumeroVilla()
        {
            NumeroVillaViewModel numeroVillaVM = new();
            var response = await _villaService.ObtenerTodos<APIResponse>(HttpContext.Session.GetString(DS.SessionToken));
            if (response != null && response.isExitoso)
            {
                numeroVillaVM.VillaList = JsonConvert.DeserializeObject<List<VillaDto>>(Convert.ToString(response.Resultado))
                    .Select(v => new SelectListItem
                    {
                        Text = v.Nombre,
                        Value = v.Id.ToString()
                    });
            }
            return View(numeroVillaVM);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CrearNumeroVilla(NumeroVillaViewModel modelo)
        {
            if (ModelState.IsValid)
            {
                var response = await _numeroVillaService.Crear<APIResponse>(modelo.NumeroVilla, HttpContext.Session.GetString(DS.SessionToken));
                if (response != null && response.isExitoso)
                {
                    TempData["exitoso"] = "Numero de Villa Creado Exitosamente";
                    return RedirectToAction(nameof(IndexNumeroVilla));
                }
                else
                {
                    if (response.errorMessages.Count > 0)
                    {
                        ModelState.AddModelError("ErrorMessages", response.errorMessages.FirstOrDefault());
                    }
                }
            }

            var res = await _villaService.ObtenerTodos<APIResponse>(HttpContext.Session.GetString(DS.SessionToken));
            if (res != null && res.isExitoso)
            {
                modelo.VillaList = JsonConvert.DeserializeObject<List<VillaDto>>(Convert.ToString(res.Resultado))
                    .Select(v => new SelectListItem
                    {
                        Text = v.Nombre,
                        Value = v.Id.ToString()
                    });
            }
            return View(modelo);

        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> ActualizarNumeroVilla(int villaNo)
        {
            NumeroVillaUpdateViewModel numeroVillaVM = new();
            var response = await _numeroVillaService.Obtener<APIResponse>(villaNo, HttpContext.Session.GetString(DS.SessionToken));
            if (response != null && response.isExitoso)
            {
                NumeroVillaDto modelo = JsonConvert.DeserializeObject<NumeroVillaDto>(Convert.ToString(response.Resultado));
                numeroVillaVM.NumeroVilla = _mapper.Map<NumeroVillaUpdateDto>(modelo);
            }

            response = await _villaService.ObtenerTodos<APIResponse>(HttpContext.Session.GetString(DS.SessionToken));
            if (response != null && response.isExitoso)
            {
                numeroVillaVM.VillaList = JsonConvert.DeserializeObject<List<VillaDto>>(Convert.ToString(response.Resultado))
                    .Select(v => new SelectListItem
                    {
                        Text = v.Nombre,
                        Value = v.Id.ToString()
                    });
                return View(numeroVillaVM);
            }

            return NotFound();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ActualizarNumeroVilla(NumeroVillaUpdateViewModel modelo)
        {
            if (ModelState.IsValid)
            {
                var response = await _numeroVillaService.Actualizar<APIResponse>(modelo.NumeroVilla, HttpContext.Session.GetString(DS.SessionToken));
                if (response != null && response.isExitoso)
                {
                    TempData["exitoso"] = "Numero de Villa Actualizada Exitosamente";
                    return RedirectToAction(nameof(IndexNumeroVilla));
                }
                else
                {
                    if (response.errorMessages.Count > 0)
                    {
                        ModelState.AddModelError("ErrorMessages", response.errorMessages.FirstOrDefault());
                    }
                }
            }

            var res = await _villaService.ObtenerTodos<APIResponse>(HttpContext.Session.GetString(DS.SessionToken));
            if (res != null && res.isExitoso)
            {
                modelo.VillaList = JsonConvert.DeserializeObject<List<VillaDto>>(Convert.ToString(res.Resultado))
                    .Select(v => new SelectListItem
                    {
                        Text = v.Nombre,
                        Value = v.Id.ToString()
                    });
            }
            return View(modelo);
        }
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> RemoverNumeroVilla(int villaNo)
        {
            NumeroVillaDeleteViewModel numeroVillaVM = new();
            var response = await _numeroVillaService.Obtener<APIResponse>(villaNo, HttpContext.Session.GetString(DS.SessionToken));
            if (response != null && response.isExitoso)
            {
                NumeroVillaDto modelo = JsonConvert.DeserializeObject<NumeroVillaDto>(Convert.ToString(response.Resultado));
                numeroVillaVM.NumeroVilla = modelo;
            }

            response = await _villaService.ObtenerTodos<APIResponse>(HttpContext.Session.GetString(DS.SessionToken));
            if (response != null && response.isExitoso)
            {
                numeroVillaVM.VillaList = JsonConvert.DeserializeObject<List<VillaDto>>(Convert.ToString(response.Resultado))
                    .Select(v => new SelectListItem
                    {
                        Text = v.Nombre,
                        Value = v.Id.ToString()
                    });
                return View(numeroVillaVM);
            }

            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoverNumeroVilla(NumeroVillaDeleteViewModel modelo)
        {
            var response = await _numeroVillaService.Remover<APIResponse>(modelo.NumeroVilla.VillaNo, HttpContext.Session.GetString(DS.SessionToken));
            if(response != null && response.isExitoso)
            {
                TempData["exitoso"] = "Numero Villa Eliminada Exitosamente";
                return RedirectToAction(nameof(IndexNumeroVilla));
            }

            TempData["error"] = "Un error ocurrio al remover";
            return View(modelo);
        }

    }
}
