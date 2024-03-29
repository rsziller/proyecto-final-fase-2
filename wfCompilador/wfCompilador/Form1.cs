﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace wfCompilador
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnAbrir_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog documento = new OpenFileDialog();
                documento.Title = "Abrir Gramática";

                if (documento.ShowDialog() == DialogResult.OK)
                {
                    if (File.Exists(documento.FileName))
                    {
                        string direccion = documento.FileName;
                        txtDocumento.Text = documento.FileName;
                        StreamReader reader = new StreamReader(direccion, System.Text.Encoding.Default);
                        txtTexto.Text = reader.ReadToEnd();
                        reader.Close();
                    }
                }
            } catch (Exception)
            {
                MessageBox.Show("No se pudo abrir el documento");
            }
            
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            SaveFileDialog guardar = new SaveFileDialog();
            try
            {
                if(guardar.ShowDialog() == DialogResult.OK)
                {
                    StreamWriter documento = File.CreateText(guardar.FileName);
                    documento.Write(txtTexto.Text);
                    documento.Flush();
                    documento.Close();
                    txtDocumento.Text = guardar.FileName;
                }
            } catch(Exception)
            {

            }
        }

        private void btnAnalizar_Click(object sender, EventArgs e)
        {
            string regexp = "";
            regexp = txtTexto.Text;

            Parser parser = new Parser();
            parser.Parse(regexp);
   
            if (parser.gramatica_aceptada())
            {
                MessageBox.Show("Gramática Aceptada", "Informativo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            } else
            {
                MessageBox.Show("Gramática No Aceptada " + parser.mensaje_error(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            

            Scanner scanner = new Scanner(regexp);

            Token nexToken;

            txtResultado.Clear();
            do
            {
                nexToken = scanner.GetToken();

                txtResultado.AppendText("Token: " + nexToken.Tag.ToString() + System.Environment.NewLine);
                txtResultado.AppendText("valor: " + nexToken.Value.ToString() + System.Environment.NewLine);
            } while (nexToken.Tag != TokenType.EOF);

            tabControl1.SelectedTab = tabControl1.TabPages["resultado"];
            // System.Console.WriteLine(result);
        }

        private void btnRegresar_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabControl1.TabPages["gramatica"];
        }
    }
}
