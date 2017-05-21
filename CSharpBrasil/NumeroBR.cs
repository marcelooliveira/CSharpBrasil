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
                return resourceManager.GetString(string.Format("Extenso{0:000}", numero));
            }
            else if (numero > 20 && numero <= 99)
            {
                if (numero % 10 == 0)
                {
                    return resourceManager.GetString(string.Format("Extenso{0:000}", numero));
                }
                else
                {
                    var unidade = numero % 10;
                    return string.Format("{0:000} e {1:000}"
                        , resourceManager.GetString(string.Format("Extenso{0:000}", (numero / 10) * 10))
                        , Extenso(unidade));
                }
            }
            else if (numero > 100 && numero <= 999)
            {
                if (numero % 100 == 0)
                {
                    return resourceManager.GetString(string.Format("Extenso{0:000}", numero));
                }
                else
                {
                    var dezena = numero % 100;
                    return string.Format("{0:000} e {1:000}"
                        , resourceManager.GetString(string.Format("Extenso{0:000}", (numero / 100) * 100))
                        , Extenso(dezena));
                }
            }
            throw new NotImplementedException();
        }
    }
}
