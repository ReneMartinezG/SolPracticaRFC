using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebPracticaRFC.Datos;
using WebPracticaRFC.Models;
using static WebPracticaRFC.Controllers.MainController;

namespace WebPracticaRFC.Controllers
{
    public class MainController : Controller
    {
        // GET: Main
        public ActionResult Index()
        {
            return View("Index");
        }

        public ActionResult irAgregar()
        {
            E_Persona objeto = new E_Persona();
            return View("FormAgregar", objeto);
        }

        public ActionResult Registar(E_Persona persona)
        {
            D_Persona objeto = new D_Persona();

            E_Persona entidad = new E_Persona();


            try
            {
                entidad = objeto.Agregar(persona);
                TempData["mensaje"] = $"RFC Creado {persona.Nombre}";


            }
            catch (Exception ex)
            {
                TempData["error"] = $"Ocurrio un error: {ex.Message}";
            }

            return View("MostrarRFC", entidad);
        }

 

        public ActionResult IrTodosRFC()
        {
            D_Persona objeto = new D_Persona();
            List<E_Persona> lista = new List<E_Persona>();
            int registros = 0;
            
            E_ViewModel viewModel = new E_ViewModel();

            try
            {
                lista = objeto.MostrarTodos();
                registros = objeto.ContarRegistros();

                 viewModel = new E_ViewModel
                 {
                    ListaPersonas = lista,
                    registros = registros

                };

                   
            } catch (Exception ex)
            {
                TempData["error"] = $"Ocurrio un error: {ex.Message}";
            }
            return View("TodosRFC", viewModel);
        }

        public ActionResult ObtenerParaEditar(int idRFC)
        {
            D_Persona datos = new D_Persona();
            E_Persona objeto = new E_Persona();

            try
            {
                objeto = datos.ObtenerParaEditar(idRFC);
            } catch (Exception ex)
            {
                TempData["error"] = $"Ocurrio un error: {ex.Message}";
            }

            return View("FormEditar", objeto);
        }

        public ActionResult EditarRegistro(E_Persona persona)
        {
            D_Persona objeto = new D_Persona();

            E_Persona entidad = new E_Persona();


            try
            {
                entidad = objeto.Editar(persona);
                TempData["mensaje"] = $"RFC Creado {persona.Nombre}";


            }
            catch (Exception ex)
            {
                TempData["error"] = $"Ocurrio un error: {ex.Message}";
            }

            return View("MostrarRFC", persona);


        }


        public ActionResult Buscar(string busqueda)
        {
            D_Persona datos = new D_Persona();
            List<E_Persona> lista = new List<E_Persona>();
            int registros = 0;
            E_ViewModel viewModel = new E_ViewModel();

            try
            {
                lista = datos.Buscar(busqueda);

                viewModel = new E_ViewModel
                {
                    ListaPersonas = lista,
                    registros = registros

                };

            }
            catch (Exception ex)
            {
                TempData["error"] = $"Ocurrio un error: {ex.Message}";
            }

            return View("TodosRFC", viewModel);
        }

        public ActionResult Eliminar(int idRFC)
        {
            D_Persona datos = new D_Persona();

            try
            {
                datos.Eliminar(idRFC);

            } catch (Exception ex)
            {
                TempData["error"] = $"Ocurrio un error: {ex.Message}";

            }

            return RedirectToAction("IrTodosRFC");
        }

        public ActionResult irGenerarRFC()
        {
            return View("FormAgregar");
        }

        public ActionResult irInicio()
        {
            return View("Index");
        }

    }
}