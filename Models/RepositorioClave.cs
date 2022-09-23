using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace inmobiliariaPestchanker.Models;

	public class RepositorioClave : RepositorioBase
	{
		public RepositorioClave() : base()
		{

		}

		public int Modificacion(Clave i)
		{
			int res = -1;
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = @"UPDATE Usuario 
				SET Nombre=@nombre, 
				Apellido=@apellido, 
				Dni=@dni,
				Email=@email, 
				Clave=@clave,
				Avatar=@avatar,
				IdRol=@idrol

				WHERE Id = @id";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@id", i.Id);
					command.Parameters.AddWithValue("@claveAnterior", i.ClaveAnterior);
					command.Parameters.AddWithValue("@claveNueva", i.ClaveNueva);
					connection.Open();
					res = command.ExecuteNonQuery();
					connection.Close();
				}
			}
			return res;
		}



	public int ModificacionClave(int id, String nuevaClave)
		{
			int res = -1;
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = @"UPDATE Usuario 
				SET  
				Clave=@clave
				
				WHERE Id = @id";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@id", id);
					command.Parameters.AddWithValue("@clave", nuevaClave);
					connection.Open();
					res = command.ExecuteNonQuery();
					connection.Close();
				}
			}
			return res;
		}




		virtual public Clave ObtenerPorId(int id)
		{
			Clave i = null;
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = @"SELECT 
				Id, Clave
				FROM usuario WHERE Id=@id";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
                    command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                    command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					if (reader.Read())
					{
						i = new Clave
						{
							Id = reader.GetInt32(0),
							ClaveAnterior = reader.GetString(1),
							
						
						};
					}
					connection.Close();
				}
			}
			return i;
        }



}


