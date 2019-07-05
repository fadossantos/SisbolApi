using System.Collections.Generic;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;


namespace SisbolApi.Models
{
    public class BDContext
    {
        private IConfiguration Configuration;

        public BDContext(IConfiguration _configuration)
        {
            Configuration = _configuration;
        }
        public InspecaoSaude buscarInspecao(string re)
        {
            var con = Configuration.GetConnectionString("BDContext");
            InspecaoSaude insp = null;
            using (SqlConnection myConnection = new SqlConnection(con))
            {
                string oString = "Select * from InspecaoSaude where Re=@Re ;";
                SqlCommand oCmd = new SqlCommand(oString, myConnection);
                oCmd.Parameters.AddWithValue("@Re", re);
                myConnection.Open();
                using (SqlDataReader oReader = oCmd.ExecuteReader())
                {                    
                    while (oReader.Read())
                    {
                        insp = new InspecaoSaude();
                        insp.Re = oReader["Re"].ToString().Trim();
                        insp.DataRealizacao = oReader["DataRealizacao"].ToString().Trim();
                        insp.Conceito = oReader["Conceito"].ToString().Trim();
                        insp.BoletimPublicacao = oReader["Publicacao"].ToString().Trim();
                    }
                    myConnection.Close();
                }
            }
            return insp;
        }

        public List<Elogio> buscarElogios(string re)
        {
            var con = Configuration.GetConnectionString("BDContext");
            List<Elogio> listaRetorno = new List<Elogio>();
            using (SqlConnection myConnection = new SqlConnection(con))
            {
                string oString = "Select * from Elogio where Re=@Re;";
                SqlCommand oCmd = new SqlCommand(oString, myConnection);
                oCmd.Parameters.AddWithValue("@Re", re);
                myConnection.Open();
                using (SqlDataReader oReader = oCmd.ExecuteReader())
                {
                    while (oReader.Read())
                    {
                        Elogio elo = new Elogio();
                        elo.Re = oReader["Re"].ToString().Trim();
                        elo.DataPublicacao = oReader["DataPublicacao"].ToString().Trim();
                        elo.BoletimPublicacao = oReader["BoletimPublicacao"].ToString().Trim();
                        elo.TextoElogio = oReader["TextoElogio"].ToString().Trim();
                        listaRetorno.Add(elo);
                    }
                    myConnection.Close();
                }
            }
            return listaRetorno;
        }

        public List<Punicao> buscarPunicoes(string re)
        {
            var con = Configuration.GetConnectionString("BDContext");
            List<Punicao> listaRetorno = new List<Punicao>();
            using (SqlConnection myConnection = new SqlConnection(con))
            {
                string oString = "Select * from Punicao where Re=@Re;";
                SqlCommand oCmd = new SqlCommand(oString, myConnection);
                oCmd.Parameters.AddWithValue("@Re", re);
                myConnection.Open();
                using (SqlDataReader oReader = oCmd.ExecuteReader())
                {
                    while (oReader.Read())
                    {
                        Punicao puni = new Punicao();
                        puni.Re = oReader["Re"].ToString().Trim();
                        puni.DataPublicacao = oReader["DataPublicacao"].ToString().Trim();
                        puni.TipoPunicao = oReader["TipoPunicao"].ToString().Trim();
                        puni.BoletimPublicacao = oReader["BoletimPublicacao"].ToString().Trim();
                        puni.TextoPunicao = oReader["TextoPunicao"].ToString().Trim();
                        listaRetorno.Add(puni);
                    }
                    myConnection.Close();
                }
            }
            return listaRetorno;
        }
    }
}