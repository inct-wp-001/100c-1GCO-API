using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace ApiLaunchBusiness
{

    //
    // Classe mApi
    //
    internal class mApi
    {
        
        private static String Base = Publicas.dynamicAPI() + Publicas.dynamicAPIBase();
        private static System.Type objType_my_api = System.Type.GetTypeFromProgID(Base);
        public static dynamic my_api = System.Activator.CreateInstance(objType_my_api);
    }

    internal static class Publicas
	{

        public static string configApiName;
        public static string configApiBase;
        public static e_Api configApiEnum;

        // Dados para instanciar Api
        public struct tp_DadosInicializacao
		{
			public string PercursoBusinessWin;
			public string FicheiroLogErros;
			public string Empresa;
			public string Utilizador;
			public string Password;

            public static tp_DadosInicializacao CreateInstance()
			{
			    tp_DadosInicializacao result = new tp_DadosInicializacao();

                result.PercursoBusinessWin = String.Empty;
                result.FicheiroLogErros = String.Empty;
                result.Empresa = String.Empty;
                result.Utilizador = String.Empty;
                result.Password = String.Empty;

                switch (configApiEnum)
                {
                    case e_Api.SageBGCOApi10:

                        result.PercursoBusinessWin = "C:\\ProgramData\\Sage\\2070\\Business";
                        result.FicheiroLogErros = "C:\\Temp";
                        result.Empresa = "DEMO_BGCO";
                        result.Utilizador = "API";
                        result.Password = "API";
                        break;

                    case e_Api.Sage1GCOApi10:

                        result.PercursoBusinessWin = "C:\\ProgramData\\Sage\\2070\\100C";
                        result.FicheiroLogErros = "C:\\Temp";
                        result.Empresa = "DEMO_1GCO";
                        result.Utilizador = "API";
                        result.Password = "API";
                        break;

                }
			    return result;
			}


		}

        // Read Api Configuratiorn
        public static Boolean readConfiguration()
        {
            string path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string fileName = path + "\\Api.ini";
            string line;
            try
            {
                // Read the file and display it line by line.  
                System.IO.StreamReader file = new System.IO.StreamReader(fileName);
                while ((line = file.ReadLine()) != null)
                {

                    Char delimiter = '=';
                    String[] values = line.Split(delimiter);
                    if (values.Length == 2)
                    {
                        switch (values[0].ToUpper())
                        {
                            case "API":
                                Publicas.configApiName = values[1];
                                switch (configApiName)
                                {
                                    case "SageBGCOApi10":
                                        Publicas.configApiEnum = e_Api.SageBGCOApi10;
                                        Publicas.configApiBase = ".BaseBusiness";
                                        break;

                                    case "Sage1GCOApi10":
                                        Publicas.configApiEnum = e_Api.Sage1GCOApi10;
                                        Publicas.configApiBase = ".Base100C";
                                        break;
                                }
                                break;
                        }
                    }
                }

                file.Close();
                return true;
            }
            catch (Exception e)
            {
                // Let the user know what went wrong.
                MessageBox.Show("Erro abertura ficheiro de Configura��es: Api.ini");
                return false;
            }
            
        }

        /**
         *
         *List all Object Properties
         * 
         */
        public static void listProperties(System.Type t, String name)
        {
            const String myPointer = " -> ";
            const String myObject = "System.Windows.Forms.UnsafeNativeMethods+IDispatch";
            //
            String myProperty = "";
            String myName = "";
            String spacer = new String('-', 50);

            Console.WriteLine("");
            Console.WriteLine(new String('=', 50));
            Console.WriteLine("Object: " + name);
            Console.WriteLine(new String('=', 50));

            dynamic o = System.Activator.CreateInstance(t);

            foreach (PropertyDescriptor prop in TypeDescriptor.GetProperties(o))
            {
                myProperty = prop.PropertyType.ToString();
                myName = (prop.Name + " " + spacer).Substring(0,50);

                if (myProperty.Equals(myObject)){
                    myProperty = "Object Type" + myPointer + prop.Name;
                }

                Console.WriteLine(myName + myPointer + myProperty);
            }



        }


        // retorna API a instanciar
        public static String dynamicAPI()
        {
            return Publicas.configApiName;
        }

        public static e_Api dynamicAPIEnum()
        {
            return Publicas.configApiEnum;
        }

        public static string dynamicAPIBase()
        {
            return Publicas.configApiBase;
        }


        // Qual Api Instanciar
        internal enum e_Api
        {
            SageBGCOApi10 = 0,
            Sage1GCOApi10 = 1
        }
        

        // Resultado Opera��o
        internal enum e_Result
        {
            Success = 0,
        }

		// Entidades utilizadas pela Api
		internal enum e_Entidade
		{
			Artigo = 1,
			Cliente,
			Fornecedor,
			Factura,
			Recibo,
			ContasPoc,
			Unidades,
			MovContabilistico,
			GerarDocumentos
		}

		// Opera��es a realizar
		internal enum e_Operacao
		{
			Inserir = 0,
			Alterar,
			Remover,
			Leitura
		}

        // Tipos de Documento
        internal enum e_TiposDocumentos
        {
            EncomendaCliente = 0, // ENC ...
            CompraFornecedor = 1  // CCF ...
        }

        // Origem do Documento
        internal enum e_OrigemDocumento
        {
            NaoIndicado = 0,
            NaoAplicavel = 1,
            Finalizado = 2,
            Preparacao = 3        
        }

    }
}