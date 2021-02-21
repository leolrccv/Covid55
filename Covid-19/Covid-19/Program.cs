using System;
using System.Globalization;
using System.IO;

namespace Covid_19
{
    class Program
    {
        static void Main(string[] args)
        {
            //Declara Arquivo , cria diretório ou não.
            ArquivoCSV arquivo = new ArquivoCSV();
            arquivo.Path = @"C:\temp\ws-c#\5by5-ativ02\Pacientes.csv";

            if (!File.Exists(arquivo.Path))
            {
                FileStream file = File.Create(arquivo.Path);
                file.Close();
            }
            
            Paciente paciente = new Paciente();
            FilaPacientes fila = new FilaPacientes();
            FilaPacientes filaPrioritaria = new FilaPacientes();

            FilaPacientes urgente = new FilaPacientes();
            FilaPacientes poucoUrgente = new FilaPacientes();
            FilaPacientes naoUrgente = new FilaPacientes();

            FilaPacientes assintomaticos = new FilaPacientes();
            int contador = 0;

            string op;
            do
            {
                Console.WriteLine(">>>BEM VINDOS AO HOSPITAL DE CAMPANHA CONTA A COVID 19<<<\n" +
                                  "1 - Cadastre um paciente\n" +
                                  "2 - Proximo da fila\n" +
                                  "3 - Chamar para internacao\n" +
                                  "4 - Encerrar programa");
                Console.Write("\n>>>");
                op = Console.ReadLine();

                switch (op)
                {
                    case "1":
                        //Fluxo Triagem Inicial
                        Console.Clear();
                        Console.Write("\nInforme o CPF: ");
                        string cpf = Console.ReadLine();

                        if (arquivo.ProcuraCPF(cpf) != -1)
                        {
                            paciente = arquivo.Leitura(cpf);
                            Imprimir(paciente);
                        }
                        else
                        {
                            paciente = Leitura(cpf);
                        }
                        //Fluxo da separação da fila
                        if (paciente.Idade() >= 60)
                        {
                            filaPrioritaria.Push(paciente);
                        }
                        else
                        {
                            fila.Push(paciente);
                        }
                        break;

                    case "2":
                        Console.Clear();

                        //Fluxo de fila
                        if (!filaPrioritaria.Vazia() && contador < 2)
                        {
                            Console.WriteLine("Chamando próximo paciente para exame...");
                            paciente = filaPrioritaria.Head;
                            filaPrioritaria.Pop();
                            contador++;

                            Imprimir(paciente);
                            Infectado(paciente, urgente, poucoUrgente, naoUrgente, assintomaticos, arquivo);
                        }
                        else if (!fila.Vazia())
                        {
                            Console.WriteLine("Chamando próximo paciente para exame...");
                            paciente = fila.Head;
                            fila.Pop();
                            contador = 0;
                            
                            Imprimir(paciente);
                            Infectado(paciente, urgente, poucoUrgente, naoUrgente, assintomaticos, arquivo);
                        }
                        else
                        {
                            Console.WriteLine("Não há ninguem na fila!");
                            contador = 0;
                        }

                        break;

                    case "3":
                        Console.Clear();
                        //if leito.vazio()
                        
                        if (!urgente.Vazia())
                        {
                            Console.WriteLine("Chamando próximo paciente para internação...\n");
                            Console.WriteLine($"Chamando o paciente {urgente.Head.Nome} para internação");
                            urgente.Pop();
                        }

                        else if (!poucoUrgente.Vazia())
                        {
                            Console.WriteLine("Chamando próximo paciente para internação...\n");
                            Console.WriteLine($"Chamando o paciente {poucoUrgente.Head.Nome} para internação");
                            poucoUrgente.Pop();
                        }

                        else if (!naoUrgente.Vazia())
                        {
                            Console.WriteLine("Chamando próximo paciente para internação...\n");
                            Console.WriteLine($"Chamando o paciente {naoUrgente.Head.Nome} para internação");
                            naoUrgente.Pop();
                        }

                        else
                        {
                            Console.WriteLine("\nSem ninguém na fila para internação!!");
                        }

                        break;

                    case "4":
                        Console.WriteLine(">>> FINALIZANDO <<<");
                        break;

                    default:
                        Console.WriteLine("Digite uma opção contida no menu !");
                        break;
                }
            } while (op != "4");
        }

        static Paciente Leitura(string cpf)
        {
            Console.WriteLine("CPF não cadastrado, insira os dados: ");

            Console.Write("Nome :");
            string nome = Console.ReadLine();

            Console.Write("Data de nascimento(dd/mm/aaaa): ");
            DateTime dataNascimento = DateTime.ParseExact(Console.ReadLine(), "dd/MM/yyyy", CultureInfo.InvariantCulture);

            Console.Write("Telefone: ");
            string telefone = Console.ReadLine();

            Paciente paciente = new Paciente
            {
                Nome = nome,
                CPF = cpf,
                DataNascimento = dataNascimento,
                Telefone = telefone,
                Proximo = null
            };

            Console.WriteLine();
            return paciente;
        }

        static void Imprimir(Paciente paciente)
        {
            Console.Clear();
            Console.WriteLine(paciente);
            Console.WriteLine();
        }

        static void Infectado(Paciente paciente, FilaPacientes urgente, FilaPacientes poucoUrgente, FilaPacientes naoUrgente, FilaPacientes assintomaticos, ArquivoCSV arquivo)
        {
            paciente.VerificaStatus();

            if (paciente.Covid)
            {
                Console.Write("\nPaciente está com sintomas?[S/N]: ");
                string sintomas = Console.ReadLine().ToUpper();

                if (sintomas == "S")
                {
                    paciente.Importancia();
                    Console.Write("\nAnalisando Urgência do paciente e adicionando em fila para internação...\n");

                    if (paciente.Comorbidade)
                    {
                        if (paciente.Periodo > 12)
                        {
                            urgente.Push(paciente);
                        }
                        else
                        {
                            poucoUrgente.Push(paciente);
                        }
                    }
                    else if (paciente.Periodo > 12)
                    {
                        poucoUrgente.Push(paciente);
                    }
                    else
                    {
                        naoUrgente.Push(paciente);
                    }
                    arquivo.Salvar(paciente);
                }
                else
                {
                    assintomaticos.Push(paciente);
                    Console.WriteLine("\nArquivando paciente...\n");
                    int posicao = arquivo.ProcuraCPF(paciente.CPF);
                    if (posicao != -1)
                    {
                        arquivo.Salvar(paciente,posicao);
                    }
                    else
                    {
                        arquivo.Salvar(paciente);
                    }
                    
                }
            }
            else
            {
                Console.WriteLine("\nArquivando paciente...\n");
                if (arquivo.ProcuraCPF(paciente.CPF) == -1)
                {
                    arquivo.Salvar(paciente);
                }
            }
        }
    }
}