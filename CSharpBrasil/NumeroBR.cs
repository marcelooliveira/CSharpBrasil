using System;
using System.Collections.Generic;
using System.Resources;
using System.Text;
using System.Globalization;
using System.Threading;

namespace CSharpBrasil
{
    public class NumeroBR
    {
        private const string NUMERO_NEGATIVO = "Número não pode ser negativo";
        private readonly ResourceManager resourceManager;
        public NumeroBR()
        {
            resourceManager = new ResourceManager(@"CSharpBrasil.Properties.Resources",
                         System.Reflection.Assembly.Load(new System.Reflection.AssemblyName("CSharpBrasil")));
        }

        public string Extenso(int numero)
        {
            if (numero < 0)
            {
                throw new ArgumentOutOfRangeException(NUMERO_NEGATIVO);
            }
            else if (numero <= 20)
            {
                return Extenso0_20(numero);
            }
            else if (numero > 20 && numero <= 999)
            {
                return Extenso21_999(numero);
            }
            throw new NotImplementedException();
        }

        private string Extenso21_999(int numero)
        {
            double numeroDigitos = Math.Floor(Math.Log10(numero));
            int potenciaDe10 = (int)Math.Pow(10, (int)numeroDigitos);
            if (numero % potenciaDe10 == 0)
            {
                return resourceManager.GetString(string.Format("Extenso{0:000}", numero));
            }
            else
            {
                string estaCasaPorExtenso = string.Empty;
                if (numero == 100)
                    estaCasaPorExtenso = resourceManager.GetString("Extenso100mais");
                else
                    estaCasaPorExtenso = resourceManager.GetString(string.Format("Extenso{0:000}", (numero / potenciaDe10) * potenciaDe10));

                var proximasCasas = numero % potenciaDe10;
                return string.Format("{0:000} e {1:000}"
                    , estaCasaPorExtenso
                    , Extenso(proximasCasas));
            }
        }

        private string Extenso0_20(int numero)
        {
            return resourceManager.GetString(string.Format("Extenso{0:000}", numero));
        }
    }
}
