
namespace Covid_19
{
    class FilaPacientes
    {
        public Paciente Head { get; set; }
        public Paciente Tail { get; set; }

        public bool Vazia()
        {
            if ((Head == null) && (Tail == null))
                return true;
            return false;
        }

        virtual public void Push(Paciente aux)
        {
            if (Vazia())
            {
                Head = aux;
                Tail = aux;
            }
            else
            {
                Tail.Proximo = aux;
                Tail = aux;
            }
        }

        public void Pop()
        {
            if (!Vazia())
            {
                Head = Head.Proximo;
                if (Head == null)
                {
                    Tail = null;
                }
            }
        }
    }
}