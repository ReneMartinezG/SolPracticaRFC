using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebPracticaRFC.Models
{
    public class E_Persona
    {
        public int idRFC { get; set; }

        public string Nombre { get; set; }

        public string ApellidoPat { get; set; }

        public string ApellidoMat { get; set; }

        public DateTime FechaNacimiento { get; set; }

        public string RFC { get; set; }

        public Boolean EsRepetido { get; set; }

        public string NombreCompleto 
        {
            get
            {
                string NombreCompleto;

                if (ApellidoMat == "x")
                {
                    NombreCompleto = Nombre + " " + ApellidoPat;
                }
                else
                {
                    NombreCompleto = Nombre + " " + ApellidoPat + " " + ApellidoMat;

                }



                return NombreCompleto;
            }
        }
    }
}