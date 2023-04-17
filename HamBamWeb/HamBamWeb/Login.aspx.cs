using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace HamBamWeb
{
    public partial class Login : System.Web.UI.Page
    {
       
            public static string GetHash(string plainText)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(plainText));
            byte[] result = md5.Hash;
            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                strBuilder.Append(result[i].ToString("x2"));//x2 định dạng chuỗi thập lục phân viết hoa
            }

            return strBuilder.ToString();
        }
        public static string DecryptMD5(string password)
        {
        string SecurityKey = "ComplexKeyHere_12121";

            MD5CryptoServiceProvider objMD5CryptoService = new MD5CryptoServiceProvider();

            //lấy giá trị mã hoá và chuyển sang byte để tính giá trị băm
            byte[] securityKeyArray = objMD5CryptoService.ComputeHash(UTF8Encoding.UTF8.GetBytes(SecurityKey));
            objMD5CryptoService.Clear();

            var objTripleDESCryptoService = new TripleDESCryptoServiceProvider();
            //Chỉ định khoá bảo mật
            objTripleDESCryptoService.Key = securityKeyArray;
            //Mode of the Crypto service is Electronic Code Book.
            objTripleDESCryptoService.Mode = CipherMode.ECB;
            //Padding Mode is PKCS7 if there is any extra byte is added.
            objTripleDESCryptoService.Padding = PaddingMode.PKCS7;

            //Transform the bytes array to resultArray

            byte[] resultArray = Encoding.ASCII.GetBytes(password);
            objTripleDESCryptoService.Clear();
            return UTF8Encoding.UTF8.GetString(resultArray);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
           
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            String sql;
            int row;
            String keyText = "HACKER";
            int[] key = setPermutationOrder(keyText);
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename =|DataDirectory|\\User.mdf;Integrated Security = True";
            conn.Open();

            sql = "SelectAccount";
            SqlCommand cmd = new SqlCommand(sql, conn) { CommandType = CommandType.StoredProcedure };



            cmd.Parameters.AddWithValue("@username", encryptTranspositionCipher(txtUsername.Text, key));
            cmd.Parameters.AddWithValue("@pw", GetHash(txtPassword.Text));



            row = (int)cmd.ExecuteScalar();
            conn.Close();
            if(row > 0)
            {
                String x = encryptTranspositionCipher(txtUsername.Text, key);



                lblResult.Text = "Đăng nhập thành công";
                lblResult.Text = ("Đăng nhập thành công<br/><br/>"
                    + "TranspositionCipher<br/>" +
                    "Tên đăng nhập trước khi mã hoá: " + txtUsername.Text + "<br/>" +
                    "Tên đăng nhập sau khi mã hoá: " + 
                    x + "<br/>" +
                    "Tên đăng nhập sau khi giải mã: " +
                    decryptTranspositionCipher(x.Trim(), key) + "<br/><br/>" 
                    + "Hàm băm<br/>" 
                    + "Mật khẩu trước khi băm: " + txtPassword.Text + "<br/>" 
                    + "Mật khẩu sau khi băm: " + GetHash(txtPassword.Text)) + "<br/>" +
                    "Mật khẩu sau khi giải mã:" + DecryptMD5(txtPassword.Text) + "<br/>";
            }
            else
            {
                lblResult.Text = "Đăng nhập thất bại";
            }

        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            String username = txtUsername.Text;
            String password = txtPassword.Text;
            String pass_hash = GetHash(password);
            String keyText = "HACKER";
            int[] key = setPermutationOrder(keyText);
            String username_encrypt = encryptTranspositionCipher(username, key);

            String sql;
            if(username.Length >0 && password.Length >0)

            {
                SqlConnection conn = new SqlConnection();
                conn.ConnectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename =|DataDirectory|\\User.mdf;Integrated Security = True";
                conn.Open();

                sql = "SelectAccountByUsername";
                SqlCommand cmd = new SqlCommand(sql, conn) {CommandType = CommandType.StoredProcedure };
                cmd.Parameters.AddWithValue("@username", username);
                SqlDataReader reader = cmd.ExecuteReader();
                if(reader.Read())
                {
                    lblResult.Text = "Tên đăng nhập đã tồn tại";
                    conn.Close();
                    return;
                }
                reader.Close();
                try
                {
                    sql = "InsertAccount";
                    SqlCommand cmd2 = new SqlCommand(sql, conn) { CommandType = CommandType.StoredProcedure };
                    cmd2.Parameters.AddWithValue("@username", username_encrypt);
                    cmd2.Parameters.AddWithValue("@pw", pass_hash);
                    cmd2.ExecuteNonQuery();
                    lblResult.Text = "Đăng ký thành công";



                }
                catch(Exception ex)
                {
                    lblResult.Text = "Lỗi: " + ex.Message;
                }

                conn.Close();

            }
            else
            {
                lblResult.Text = "Tên đăng nhập và mật khẩu không được để trống";
            }
        }
        public int[] setPermutationOrder(String key)
        {
            int len = key.Length;
            int[] keyArray = new int[len];
            int[] keyArrayCoppy = new int[len];
            for (int i = 0; i < len; i++)
            {
                keyArray[i] = key[i];
                keyArrayCoppy[i] = key[i];
            }
            for (int i = 0; i < len; i++)
            {
                for (int j = i + 1; j < len; j++)
                {
                    if (keyArray[i] > keyArray[j])
                    {
                        int temp = keyArray[i];
                        keyArray[i] = keyArray[j];
                        keyArray[j] = temp;
                    }
                }
            }

            for (int i = 0; i < len; i++)
            {
                for (int j = 0; j < len; j++)
                {
                    if (keyArray[i] == keyArrayCoppy[j])
                    {
                        keyArrayCoppy[j] = i + 1;
                    }
                }
            }
            

            return keyArrayCoppy;
        }
        public String encryptTranspositionCipher(String plaintext, int[] key)
        {
            int numColumns = key.Length;
            int numRows = (int)Math.Ceiling((double)plaintext.Length / numColumns);
            char[,] matrix = new char[numRows, numColumns];

            int k = 0;
            for (int i = 0; i < numRows; i++)
            {
                for (int j = 0; j < numColumns; j++)
                {
                    if (k < plaintext.Length)
                    {
                        matrix[i,j] = plaintext[k];
                        k++;
                    }else{
                        matrix[i,j] = '_';
                        k++;
                    }
                    
                }
            }

            StringBuilder ciphertext = new StringBuilder();
            for (int idx = 0; idx < key.Length; idx++)
            {
                int column = key[idx] - 1;
                for (int row = 0; row < numRows; row++)
                {     
                    if (matrix[row, column] != '\0')
                    {       
                        ciphertext.Append(matrix[row, column]);
                    }
                }
            }

            return ciphertext.ToString();
        }
        public static String decryptTranspositionCipher(String ciphertext, int[] key)
        {
            int numColumns = key.Length;
            int numRows = (int)Math.Ceiling((double)ciphertext.Length / numColumns);
            char[,] matrix = new char[numRows, numColumns];

            int k = 0;
            for (int idx = 0; idx < key.Length; idx ++)
            {
                int column = key[idx] - 1;
                for (int row = 0; row < numRows; row++)
                {
                    if (k < ciphertext.Length)
                    {
                        matrix[row, column] = ciphertext[k];
                        k++;
                    }else{
                        matrix[i,j] = '_';
                        k++;
                    }
                   
                }
            }

            StringBuilder plaintext = new StringBuilder();
            for (int row = 0; row < numRows; row++)
            {
                for (int column = 0; column < numColumns; column++)
                {
                    if (matrix[row, column] != '_')
                    {
                        plaintext.Append(matrix[row, column]);
                    }
                }
            }

            return plaintext.ToString();
        }

        
    }
}
