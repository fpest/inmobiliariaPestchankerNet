using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace inmobiliariaPestchanker.Models;

	public class RepositorioPago : RepositorioBase
	{
		public RepositorioPago() : base()
		{

		}

		public int Alta(Pago i)
		{
			int res = -1;
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = @"INSERT INTO Pago (FechaPago, Importe, IdContrato)
				 VALUES (@FechaPago, @Importe, @IdContrato);" +
					"SELECT LAST_INSERT_ID();";//devuelve el id insertado (LAST_INSERT_ID para mysql)
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@FechaPago", i.FechaPago);
					command.Parameters.AddWithValue("@Importe", i.Importe);
					command.Parameters.AddWithValue("@IdContrato", i.IdContrato);
					connection.Open();
					res = Convert.ToInt32(command.ExecuteScalar());
                    i.Id = res;
                    connection.Close();
				}
			}
			return res;
		}

		
		public int Baja(int id)
		{
			int res = -1;
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = $"DELETE FROM Pago WHERE Id = @id";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@id", id);
					connection.Open();
					res = command.ExecuteNonQuery();
					connection.Close();
				}
			}
			return res;
		}

		
		public int Modificacion(PagoLista i)
		{
			int res = -1;
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{

				string sql = @"UPDATE pago SET 
				FechaPago=@FechaPago, 
				Importe=@Importe, 
				IdContrato=@IdContrato 
				WHERE Id = @id";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@FechaPago", i.FechaPago);
					command.Parameters.AddWithValue("@Importe", i.Importe);
					command.Parameters.AddWithValue("@IdContrato", i.IdContrato);
					command.Parameters.AddWithValue("@id", i.Id);
					connection.Open();
					res = command.ExecuteNonQuery();
					connection.Close();
				}
			}
			return res;
		}

		
		

	virtual public PagoLista ObtenerPorId(int id)
		{
			PagoLista i = null;
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql =  @"SELECT pag.Id, FechaPago, Importe, IdContrato
				 , inq.Apellido, inq.Nombre, inm.Direccion, prop.Apellido, prop.Nombre,
				 cont.Precio,cont.FechaInicio, cont.FechaFin 
				 FROM pago pag
				 join contrato cont on pag.IdContrato = cont.Id
				 join inmueble inm on cont.IdInmueble = inm.Id
				 join inquilino inq on cont.IdInquilino = inq.Id
				 join propietario prop on  inm.IdPropietario = prop.Id
				 WHERE pag.Id=@id"; 
				
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
                    command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                    command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					if (reader.Read())
					{
						i = new PagoLista
						{
							Id = reader.GetInt32(0),
							FechaPago = reader.GetDateTime(1),
							Importe = reader.GetDecimal(2),
							IdContrato = reader.GetInt32(3),						
							InqApellido = reader.GetString(4),
							InqNombre = reader.GetString(5),
							Direccion = reader.GetString(6),
							PropApellido = reader.GetString(7),
							PropNombre = reader.GetString(8),
							PrecioContrato = reader.GetDecimal(9),
							FechaInicioContrato=reader.GetDateTime(10),
							FechaFinContrato=reader.GetDateTime(11),
				
						};
					};
					connection.Close();
				}
			}
			return i;
			}

			virtual public IList<PagoLista> ObtenerPorContrato(int idContrato)
		{
			IList<PagoLista> i = new List<PagoLista>();
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = @"SELECT pag.Id, FechaPago, Importe, IdContrato
				 , inq.Apellido, inq.Nombre, inm.Direccion, prop.Apellido, prop.Nombre,
				 cont.Precio,cont.FechaInicio, cont.FechaFin 
				 FROM pago pag
				 join contrato cont on pag.IdContrato = cont.Id
				 join inmueble inm on cont.IdInmueble = inm.Id
				 join inquilino inq on cont.IdInquilino = inq.Id
				 join propietario prop on  inm.IdPropietario = prop.Id
				 WHERE cont.Id=@idContrato";
				
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
                    command.Parameters.Add("@idContrato", MySqlDbType.Int32).Value = idContrato;
                    command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						PagoLista pl = new PagoLista
						{
							Id = reader.GetInt32(0),
							FechaPago = reader.GetDateTime(1),
							Importe = reader.GetDecimal(2),
							IdContrato = reader.GetInt32(3),						
							InqApellido = reader.GetString(4),
							InqNombre = reader.GetString(5),
							Direccion = reader.GetString(6),
							PropApellido = reader.GetString(7),
							PropNombre = reader.GetString(8),
							PrecioContrato = reader.GetDecimal(9),
							FechaInicioContrato=reader.GetDateTime(10),
							FechaFinContrato=reader.GetDateTime(11),
				
						};
						i.Add(pl);

					};
					connection.Close();
				}
			}
			return i;
			}

virtual public Decimal ObtenerMontoPagadoPorContrato(int idContrato)
		{
			
		
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{

					Decimal monto=0; 
				string sql = @"SELECT Sum(Importe) 
				 FROM pago pag
				 join contrato cont on pag.IdContrato = cont.Id
				 WHERE cont.Id=@idContrato";
				
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
                    command.Parameters.Add("@idContrato", MySqlDbType.Int32).Value = idContrato;
                    command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					if (reader.Read())
					{
							monto = reader.GetDecimal(0);
					};
					connection.Close();
				}
				return monto;
			}
			
			}









					virtual public PagoLista ObtenerInfoPorContrato(int idContrato)
		{
			PagoLista i = null;
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = @"SELECT 
				cont.Id, 
				inq.Nombre, 
				inq.Apellido, 
				inm.Direccion, 
				prop.Apellido, 
				prop.Nombre, 
				cont.Precio, 
				cont.FechaInicio, 
				cont.FechaFin
				 FROM contrato cont
				 join inquilino inq on cont.IdInquilino = inq.Id
                 join inmueble inm on cont.IdInmueble = inm.Id
                 join propietario prop on prop.Id = inm.IdPropietario
         		 WHERE cont.Id=@idContrato";
				
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
                    command.Parameters.Add("@idContrato", MySqlDbType.Int32).Value = idContrato;
                    command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					if (reader.Read())
					{
						i = new PagoLista
						{   IdContrato = reader.GetInt32(0),
							InqNombre = reader.GetString(1),
							InqApellido = reader.GetString(2),
							Direccion = reader.GetString(3),
							PropApellido = reader.GetString(4),
							PropNombre = reader.GetString(5),
							PrecioContrato = reader.GetDecimal(6),
							FechaInicioContrato=reader.GetDateTime(7),
							FechaFinContrato=reader.GetDateTime(8),
							Id = 0,
							FechaPago = DateTime.Today,
							Importe = 0
													
						};
					};
					connection.Close();
				}
			}
			return i;
			}

			
			
        





/*
		public IList<Inquilino> ObtenerLista(int paginaNro = 1, int tamPagina = 10)
		{
			IList<Inquilino> res = new List<Inquilino>();
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = @$"
					SELECT Id, Nombre, Apellido, Dni, Telefono, Email
					FROM Inquilino
					LIMIT {tamPagina} OFFSET {(paginaNro - 1) * tamPagina}
				";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						Inquilino i = new Inquilino
						{
							Id = reader.GetInt32(0),
							Nombre = reader.GetString(1),
							Apellido = reader.GetString(2),
							Dni = reader.GetString(3),
							Telefono = reader["Telefono"].ToString(),
							Email = reader.GetString(5),
							
						};
						res.Add(i);
					}
					connection.Close();
				}
			}
			return res;
		}


		virtual public Inquilino ObtenerPorId(int id)
		{
			Inquilino i = null;
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = $"SELECT Id, Nombre, Apellido, Dni, Telefono, Email FROM Inquilino" +
					$" WHERE Id=@id";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
                    command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                    command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					if (reader.Read())
					{
						i = new Inquilino
						{
							Id = reader.GetInt32(0),
							Nombre = reader.GetString(1),
							Apellido = reader.GetString(2),
							Dni = reader.GetString(3),
							Telefono = reader.GetString(4),
							Email = reader.GetString(5),
							
						};
					}
					connection.Close();
				}
			}
			return i;
        }
*/


/*
        public Propietario ObtenerPorEmail(string email)
        {
            Propietario p = null;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string sql = $"SELECT IdPropietario, Nombre, Apellido, Dni, Telefono, Email, Clave FROM Propietarios" +
                    $" WHERE Email=@email";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.Add("@email", SqlDbType.VarChar).Value = email;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        p = new Propietario
                        {
                            IdPropietario = reader.GetInt32(0),
                            Nombre = reader.GetString(1),
                            Apellido = reader.GetString(2),
                            Dni = reader.GetString(3),
                            Telefono = reader.GetString(4),
                            Email = reader.GetString(5),
                            Clave = reader.GetString(6),
                        };
                    }
                    connection.Close();
                }
            }
            return p;
        }

        public IList<Propietario> BuscarPorNombre(string nombre)
        {
            List<Propietario> res = new List<Propietario>();
            Propietario p = null;
			nombre = "%" + nombre + "%";
			using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string sql = $"SELECT IdPropietario, Nombre, Apellido, Dni, Telefono, Email, Clave FROM Propietarios" +
                    $" WHERE Nombre LIKE @nombre OR Apellido LIKE @nombre";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.Add("@nombre", SqlDbType.VarChar).Value = nombre;
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        p = new Propietario
                        {
                            IdPropietario = reader.GetInt32(0),
                            Nombre = reader.GetString(1),
                            Apellido = reader.GetString(2),
                            Dni = reader.GetString(3),
                            Telefono = reader.GetString(4),
                            Email = reader.GetString(5),
                            Clave = reader.GetString(6),
                        };
                        res.Add(p);
                    }
                    connection.Close();
                }
            }
            return res;
        }

	}

public IList<Contrato> ObtenerTodos()
		{
			IList<Contrato> res = new List<Contrato>();
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = @"SELECT cont.Id, IdInmueble, IdInquilino, FechaInicio,
				 FechaFin, Precio, inq.Apellido, inq.Nombre, inm.Direccion 
				 FROM contrato cont
				 join inmueble inm on cont.IdInmueble = inm.Id
				 join inquilino inq on cont.IdInquilino = inq.Id";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						Contrato i = new Contrato
						{
							Id = reader.GetInt32(0),
							IdInmueble = reader.GetInt32(1),
							IdInquilino = reader.GetInt32(2),
							FechaInicio = reader.GetDateTime(3),						
							FechaFin = reader.GetDateTime(4),
							Precio = reader.GetDecimal(5),
							Inquilino = new Inquilino{
								Id = reader.GetInt32(2),
								Nombre = reader.GetString(7),
								Apellido = reader.GetString(6),
							},
							Inmueble = new Inmueble{
								Id = reader.GetInt32(1),
								Direccion = reader.GetString(8),
														
							},

							
						};
						res.Add(i);
					}
					connection.Close();
				}
			}
			return res;
		}





*/


}


