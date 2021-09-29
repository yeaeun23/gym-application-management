﻿using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;

public class Util
{
	public Util()
	{
	}

	public static DataTable ExecuteQuery(SqlCommand cmd, string action)
	{
		string conString = ConfigurationManager.AppSettings["system6strConn"];

		using (SqlConnection con = new SqlConnection(conString))
		{
			cmd.Connection = con;

			switch (action)
			{
				case "SELECT":
					using (SqlDataAdapter sda = new SqlDataAdapter())
					{
						sda.SelectCommand = cmd;

						using (DataTable dt = new DataTable())
						{
							sda.Fill(dt);
							return dt;
						}
					}
				case "UPDATE":
				case "INSERT":
				case "DELETE":
					con.Open();
					cmd.ExecuteNonQuery();
					con.Close();
					break;
			}

			return null;
		}
	}

	public static void SaveLog(string filename, string message)
	{
		string curdir = ConfigurationManager.AppSettings["logpath"];

		if (!Directory.Exists(curdir))
			Directory.CreateDirectory(curdir);

		string strFileName = curdir + "\\" + filename + "_" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
		FileStream fo = null;
		StreamWriter sw = null;

		try
		{
			fo = new FileStream(strFileName, FileMode.Append);
			sw = new StreamWriter(fo);

			sw.WriteLine(message);
			sw.Close();
			fo.Close();
		}
		catch (Exception)
		{
			if (sw != null)
				sw.Close();

			if (fo != null)
				fo.Close();
		}
	}
}