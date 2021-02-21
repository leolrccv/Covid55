using System;
using System.Globalization;
using System.IO;

namespace Covid_19
{
    class ArquivoCSV
    {
        public string Path { get; set; }

        public int ProcuraCPF(string cpf)
        {
            string[] lines = File.ReadAllLines(Path);
            if (lines.Length > 1)
            {
                for (int i = 1; i < lines.Length; i++)
                {
                    string line = lines[i];
                    string[] pacienteCSV = line.Split(',');
                    if (pacienteCSV[0] == cpf)
                    {
                        return i;
                    }
                }
            }
            return -1;
        }

        public Paciente Leitura(string cpf)
        {
            string[] lines = File.ReadAllLines(Path);

            for (int i = 1; i < lines.Length; i++)
            {
                string line = lines[i];
                string[] pacienteCSV = line.Split(',');
                if (pacienteCSV[0] == cpf)
                {
                    Paciente paciente = new Paciente
                    {
                        CPF = cpf,
                        Nome = pacienteCSV[1],
                        DataNascimento = DateTime.ParseExact(pacienteCSV[2], "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        Telefone = pacienteCSV[3],
                        Proximo = null
                    };
                    return paciente;
                }
            }
            return null;
        }
        public void Salvar(Paciente paciente)
        {
            string linepaciente = $"{paciente.CPF}," +
                                  $"{paciente.Nome}," +
                                  $"{paciente.DataNascimento.ToString("dd/MM/yyyy")}," +
                                  $"{paciente.Telefone}," +
                                  $"{paciente.Comorbidade}," +
                                  $"{paciente.Periodo}," +
                                  $"{paciente.Covid}";
            string[] lines = File.ReadAllLines(Path);

            StreamWriter sw = File.AppendText(Path);
            sw.Write("\n" + linepaciente);
            sw.Close();
        }
        public void Salvar(Paciente paciente, int i)
        {
            string linepaciente = $"{paciente.CPF}," +
                                  $"{paciente.Nome}," +
                                  $"{paciente.DataNascimento.ToString("dd/MM/yyyy")}," +
                                  $"{paciente.Telefone}," +
                                  $"{paciente.Comorbidade}," +
                                  $"{paciente.Periodo}," +
                                  $"{paciente.Covid}";
            string[] lines = File.ReadAllLines(Path);



            string line = lines[i];
            string[] pacienteCSV = line.Split(',');
            
            if (pacienteCSV[4] != paciente.Comorbidade.ToString())
            {
                pacienteCSV[4].Replace(pacienteCSV[4], paciente.Comorbidade.ToString());
            }
            if (pacienteCSV[5] != paciente.Periodo.ToString())
            {
                pacienteCSV[5].Replace(pacienteCSV[5], paciente.Periodo.ToString());
            }
            if (pacienteCSV[6] != paciente.Covid.ToString())
            {
                pacienteCSV[6].Replace(pacienteCSV[6], paciente.Covid.ToString());
            }
        }
    }
}