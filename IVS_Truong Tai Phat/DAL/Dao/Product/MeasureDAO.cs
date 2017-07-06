using Core.Common;
using DTO.Measure;
using DTO.Product.Measure;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;

namespace DAL.Dao.Product
{
    public class MeasureDAO
    {
        private static string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
        public static CommonData.ReturnCode SearchData(MeasureDTO inputDto, out List<MeasureDTO> list)
        {
            list = new List<MeasureDTO>();
            CommonData.ReturnCode returnCode = CommonData.ReturnCode.Success;
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    string sql = "SELECT ms.`id`, ms.`code`, ms.`name`, ms.`description` FROM `product_measure` ms WHERE TRUE";

                    #region Clause
                    if (inputDto.id != null)
                    {
                        sql += " AND ms.`id` = @ID ";
                        cmd.Parameters.AddWithValue("@ID", inputDto.id);
                    }

                    if (inputDto.code.IsNotNullOrEmpty())
                    {
                        sql += " AND ms.`code` LIKE CONCAT('%',@Code,'%') ";
                        cmd.Parameters.AddWithValue("@Code", inputDto.code);
                    }

                    if (inputDto.name.IsNotNullOrEmpty())
                    {
                        sql += " AND ms.`name` LIKE CONCAT('%',@Name,'%') ";
                        cmd.Parameters.AddWithValue("@Name", inputDto.name);
                    }
                    #endregion

                    sql += " ORDER BY ms.`updated_datetime` DESC";
                    sql += " LIMIT  @start,20";
                    if (inputDto.page > 1)
                    {
                        cmd.Parameters.AddWithValue("@start", 20 * (inputDto.page - 1));
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@start", 0);
                    }

                    cmd.Connection = con;
                    cmd.CommandText = sql;
                    using (MySqlDataAdapter sda = new MySqlDataAdapter())
                    {
                        sda.SelectCommand = cmd;
                        DataTable dt = new DataTable();
                        sda.Fill(dt);
                        if (dt.Rows.Count != 0)
                        {
                            foreach (DataRow category in dt.Rows)
                            {
                                MeasureDTO dto = new MeasureDTO();

                                dto.id = int.Parse(category["id"].ToString());
                                dto.code = category["code"].ToString();
                                dto.name = category["name"].ToString();
                                dto.description = category["description"].ToString();

                                list.Add(dto);
                            }
                            returnCode = 0;
                        }
                        else
                        {
                            returnCode = CommonData.ReturnCode.UnSuccess;
                        }
                    }
                }
            }

            return returnCode;
        }

        public static CommonData.ReturnCode SelectSimpleData(out List<MeasureDropdownlistDTO> list)
        {
            list = new List<MeasureDropdownlistDTO>();

            CommonData.ReturnCode returnCode = CommonData.ReturnCode.Success;
            string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                using (MySqlCommand cmd = new MySqlCommand("SELECT id, code, name FROM product_measure", con))
                {
                    using (MySqlDataAdapter sda = new MySqlDataAdapter())
                    {
                        sda.SelectCommand = cmd;
                        DataTable dt = new DataTable();
                        sda.Fill(dt);
                        if (dt.Rows.Count != 0)
                        {
                            foreach (DataRow item in dt.Rows)
                            {
                                MeasureDropdownlistDTO dto = new MeasureDropdownlistDTO();

                                dto.id = int.Parse(item["id"].ToString());
                                dto.code = item["code"].ToString();
                                dto.name = item["name"].ToString();

                                list.Add(dto);
                            }
                        }
                    }
                }
            }

            return returnCode;
        }

        public static int CountData(MeasureDTO dto)
        {
            int count = 0;
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    string sql = " SELECT COUNT(*) FROM product_measure ms WHERE TRUE ";

                    #region Where Clause
                    if (dto.id != null)
                    {
                        sql += " AND ms.`id` = @ID ";
                        cmd.Parameters.AddWithValue("@ID", dto.id);
                    }

                    if (dto.code.IsNotNullOrEmpty())
                    {
                        sql += " AND ms.`code` LIKE CONCAT('%',@Code,'%') ";
                        cmd.Parameters.AddWithValue("@Code", dto.code);
                    }

                    if (dto.name.IsNotNullOrEmpty())
                    {
                        sql += " AND ms.`name` LIKE CONCAT('%',@Name,'%') ";
                        cmd.Parameters.AddWithValue("@Name", dto.name);
                    }
                    #endregion

                    con.Open();
                    cmd.Connection = con;
                    cmd.CommandText = sql;
                    count = int.Parse(cmd.ExecuteScalar().ToString());
                }
            }

            return count;
        }

        public static CommonData.ReturnCode InsertData(MeasureDTO dto)
        {
            CommonData.ReturnCode returnCode = CommonData.ReturnCode.Success;
            using (MySqlConnection connect = new MySqlConnection(constr))
            {
                using (MySqlCommand command = new MySqlCommand(
                    @"INSERT INTO product_measure(`code`, `name`,
                                                    `description`, `created_datetime`, `created_by`,
                                                    `updated_datetime`, `updated_by`)
                                                    VALUES 
                                                    (@code, @name, @description,
                                                     SYSDATE(), @created_by,
                                                     SYSDATE(), @updated_by)", connect))
                {
                    command.Parameters.AddWithValue("@code", dto.code);
                    command.Parameters.AddWithValue("@name", dto.name);
                    command.Parameters.AddWithValue("@description", dto.description);
                    command.Parameters.AddWithValue("@created_by", dto.created_by);
                    command.Parameters.AddWithValue("@updated_by", dto.updated_by);

                    connect.Open();
                    command.ExecuteNonQuery();
                }
            }
            return returnCode;
        }

        public static CommonData.ReturnCode UpdateData(MeasureDTO dto)
        {
            CommonData.ReturnCode returnCode = CommonData.ReturnCode.Success;
            using (MySqlConnection connect = new MySqlConnection(constr))
            {
                using (MySqlCommand command = new MySqlCommand(
                    @"UPDATE product_measure pm 
                            SET
                            pm.`code` = @code,
                            pm.`name` = @name,
                            pm.`description` = @description,
                            pm.`updated_datetime` = SYSDATE(),
                            pm.`updated_by` = @updated_by WHERE pm.`id` = @ID", connect))
                {
                    try
                    {
                        command.Parameters.AddWithValue("@code", dto.code);
                        command.Parameters.AddWithValue("@name", dto.name);
                        command.Parameters.AddWithValue("@description", dto.description);
                        command.Parameters.AddWithValue("@updated_by", dto.updated_by);
                        command.Parameters.AddWithValue("@ID", dto.id);

                        connect.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        returnCode = CommonData.ReturnCode.UnSuccess;
                    }
                }
            }
            return returnCode;
        }

        public static CommonData.ReturnCode DeleteData(int id)
        {
            CommonData.ReturnCode returnCode = CommonData.ReturnCode.Success;
            string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
            using (MySqlConnection connect = new MySqlConnection(constr))
            {
                using (MySqlCommand command = new MySqlCommand(
                    @"DELETE FROM product_measure WHERE id = @ID", connect))
                {
                    try
                    {
                        command.Parameters.AddWithValue("@ID", id);
                        connect.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        returnCode = CommonData.ReturnCode.UnSuccess;
                    }
                }
            }
            return returnCode;
        }
    }
}
