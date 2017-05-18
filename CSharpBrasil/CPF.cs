using System;
using System.Collections.Generic;
using System.Linq;

namespace CSharpBrasil
{
    public class CPF
    {
        private const int ONZE = 11;
        private const int NOVE = 9;

        public static bool IsValid(string cpf)
        {
            string trechoCPF =
                cpf.Substring(0, 9);

            int digito1 = GetDigito(trechoCPF);
            int digito2 = GetDigito(trechoCPF + digito1.ToString());

            return cpf == trechoCPF + digito1.ToString() + digito2.ToString();
        }

        private static int GetDigito(string trechoCPF)
        {
            List<int> digitos =
                trechoCPF
                .ToCharArray()
                .Select(c => int.Parse(c.ToString()))
                .ToList();

            List<int> multiplicadores =
                Enumerable
                .Range(2, digitos.Count())
                .OrderByDescending(m => m)
                .ToList();

            int soma = 0;
            for (int i = 0; i < trechoCPF.Count(); i++)
                soma += digitos[i] * multiplicadores[i];

            int resto = soma % ONZE;

            int subtracao = ONZE - resto;

            int digito = 0;
            if (subtracao > NOVE)
                digito = 0;
            else
                digito = subtracao;
            return digito;
        }
    }
}
