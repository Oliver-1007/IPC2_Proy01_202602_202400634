
using System;
using System.Diagnostics;
using System.Text;
using ChapinWarriors.Models.Entidades;

namespace ChapinWarriors.Services
{
    public class GeneradorGraphviz
    {
        // FUNCION DEL TIPO STRING ENCARGADA DE GENERAR EL CODIGO DOT
        public string GenerarDot(Ciudad ciudad, ResultadoMision resultado)
        {
            var sb = new StringBuilder();
            sb.AppendLine("digraph Ciudad {");
            sb.AppendLine("  rankdir=TB;");
            sb.AppendLine("  node [shape=plaintext, fontname=\"Arial\"];");
            sb.AppendLine();

            string titulo = resultado.TipoMision == "rescate" ? "Ruta de rescate:" : "Ruta de extracción de recurso:";

            sb.AppendLine("  MainTable [label=<");
            sb.AppendLine("    <TABLE BORDER=\"0\" CELLBORDER=\"0\" CELLSPACING=\"0\" CELLPADDING=\"0\">");
            sb.AppendLine($"      <TR><TD ALIGN=\"CENTER\"><FONT POINT-SIZE=\"14\"><B>{titulo}</B></FONT></TD></TR>");
            sb.AppendLine("      <TR><TD HEIGHT=\"10\"></TD></TR>");
            sb.AppendLine("      <TR>");
            sb.AppendLine("        <TD ALIGN=\"CENTER\">");
            sb.AppendLine("          <TABLE BORDER=\"0\" CELLBORDER=\"1\" CELLSPACING=\"0\" CELLPADDING=\"6\">");

            // Enumeracion de las columnas
            sb.Append("            <TR><TD BORDER=\"0\"></TD>");
            for (int c = 1; c <= ciudad.Columnas; c++)
            {
                sb.Append($"<TD BORDER=\"0\" ALIGN=\"CENTER\"><FONT POINT-SIZE=\"8\">{c}</FONT></TD>");
            }
            sb.AppendLine("</TR>");

            // Filas de la malla
            for (int f = 0; f < ciudad.Filas; f++)
            {
                // Enumeracion de las filas
                sb.Append($"            <TR><TD BORDER=\"0\" ALIGN=\"CENTER\"><FONT POINT-SIZE=\"8\">{f + 1}</FONT></TD>");
                // Columnas de la malla
                for (int c = 0; c < ciudad.Columnas; c++)
                {
                    var celda = ciudad.ObtenerCelda(f, c);
                    string color = (EstaEnRuta(resultado, f, c) && celda.Tipo == TipoCelda.Camino) ? "khaki" : ObtenerColor(celda);
                    sb.Append($"<TD BGCOLOR=\"{color}\" WIDTH=\"15\" HEIGHT=\"15\"></TD>");
                }
                sb.AppendLine("</TR>");
            }

            sb.AppendLine("          </TABLE>");
            sb.AppendLine("        </TD>");
            sb.AppendLine("      </TR>");
            sb.AppendLine("      <TR><TD HEIGHT=\"15\"></TD></TR>");

            // Descripcion de los colores
            sb.AppendLine("      <TR>");
            sb.AppendLine("        <TD ALIGN=\"CENTER\">");
            sb.AppendLine("          <TABLE BORDER=\"0\" CELLBORDER=\"0\" CELLSPACING=\"5\" CELLPADDING=\"0\">");
            sb.AppendLine("            <TR><TD BGCOLOR=\"black\" WIDTH=\"15\" HEIGHT=\"15\" BORDER=\"1\"></TD><TD ALIGN=\"LEFT\"><FONT POINT-SIZE=\"10\">Intransitable</FONT></TD></TR>");
            sb.AppendLine("            <TR><TD BGCOLOR=\"green3\" WIDTH=\"15\" HEIGHT=\"15\" BORDER=\"1\"></TD><TD ALIGN=\"LEFT\"><FONT POINT-SIZE=\"10\">Punto de entrada</FONT></TD></TR>");
            sb.AppendLine("            <TR><TD BGCOLOR=\"white\" WIDTH=\"15\" HEIGHT=\"15\" BORDER=\"1\"></TD><TD ALIGN=\"LEFT\"><FONT POINT-SIZE=\"10\">Camino</FONT></TD></TR>");
            sb.AppendLine("            <TR><TD BGCOLOR=\"red2\" WIDTH=\"15\" HEIGHT=\"15\" BORDER=\"1\"></TD><TD ALIGN=\"LEFT\"><FONT POINT-SIZE=\"10\">Unidad militar</FONT></TD></TR>");
            sb.AppendLine("            <TR><TD BGCOLOR=\"steelblue\" WIDTH=\"15\" HEIGHT=\"15\" BORDER=\"1\"></TD><TD ALIGN=\"LEFT\"><FONT POINT-SIZE=\"10\">Unidad civil</FONT></TD></TR>");
            sb.AppendLine("            <TR><TD BGCOLOR=\"gray\" WIDTH=\"15\" HEIGHT=\"15\" BORDER=\"1\"></TD><TD ALIGN=\"LEFT\"><FONT POINT-SIZE=\"10\">Recurso</FONT></TD></TR>");
            sb.AppendLine("          </TABLE>");
            sb.AppendLine("        </TD>");
            sb.AppendLine("      </TR>");
            sb.AppendLine("      <TR><TD HEIGHT=\"15\"></TD></TR>");

            // Detalles de la mision
            string tipoLabel = resultado.TipoMision == "rescate" ? "Tipo de misión: rescate" : "Tipo de misión: extracción de recursos";
            
            string objetivoLabel = "";
            if (resultado.Objetivo != null)
            {
                int fila1Based = resultado.Objetivo.Fila + 1;
                int col1Based = resultado.Objetivo.Columna + 1;
                objetivoLabel = resultado.TipoMision == "rescate" 
                    ? $"Unidad civil rescatada: {fila1Based},{col1Based}" 
                    : $"Recurso extraído: {fila1Based},{col1Based}";
            }

            string robotLabel = "";
            if (resultado.TipoMision == "rescate")
            {
                robotLabel = $"Robot utilizado: {resultado.RobotUtilizado} (ChapinRescue)";
            }
            else
            {
                robotLabel = $"Robot utilizado: {resultado.RobotUtilizado} (ChapinFighter - Capacidad de combate inicial {resultado.CapacidadInicial}, Capacidad de combate final {resultado.CapacidadFinal})";
            }

            sb.AppendLine("      <TR>");
            sb.AppendLine("        <TD ALIGN=\"CENTER\">");
            sb.AppendLine("          <FONT POINT-SIZE=\"11\">");
            sb.AppendLine($"            {tipoLabel}<BR/>");
            if (!string.IsNullOrEmpty(objetivoLabel))
            {
                sb.AppendLine($"            {objetivoLabel}<BR/>");
            }
            sb.AppendLine($"            {robotLabel}<BR/>");
            sb.AppendLine("          </FONT>");
            sb.AppendLine("        </TD>");
            sb.AppendLine("      </TR>");

            sb.AppendLine("    </TABLE>");
            sb.AppendLine("  >];");
            sb.AppendLine("}");

            return sb.ToString();
        }

        // FUNCION AUXILIAR PARA EL CODIGO DOT
        private bool EstaEnRuta(ResultadoMision resultado, int f, int c)
        {
            for (int i = 0; i < resultado.Ruta.Longitud; i++)
            {
                var celda = (Celda)resultado.Ruta[i];
                if (celda.Fila == f && celda.Columna == c) return true;
            }
            return false;
        }

        // FUNCION AUXILIAR PARA EL CODIGO DOT
        private string ObtenerColor(Celda celda)
        {
            return celda.Tipo switch
            {
                TipoCelda.Intransitable => "black",
                TipoCelda.PuntoEntrada => "green3",
                TipoCelda.Camino => "white",
                TipoCelda.UnidadMilitar => "red2",
                TipoCelda.UnidadCivil => "steelblue",
                TipoCelda.Recurso => "gray",
                _ => "white"
            };
        }

        // FUNCION ENCARGADA DE RENDERIZAR EL CODIGO DOT A IMAGEN EN FORMATO SVG
        public string GenerarSvg(string dotContent)
        {
            try
            {
                string exePath = "dot";
                if (!TestDotPath(exePath))
                {
                    string defaultPath = @"C:\Program Files\Graphviz\bin\dot.exe";
                    if (System.IO.File.Exists(defaultPath))
                    {
                        exePath = defaultPath;
                    }
                }

                var startInfo = new ProcessStartInfo
                {
                    FileName = exePath,
                    Arguments = "-Tsvg",
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (var process = new Process { StartInfo = startInfo })
                {
                    process.Start();
                    using (var writer = process.StandardInput)
                    {
                        writer.Write(dotContent);
                    }
                    string svg = process.StandardOutput.ReadToEnd();
                    string error = process.StandardError.ReadToEnd();
                    process.WaitForExit();
                    if (process.ExitCode == 0)
                    {
                        int svgStartIndex = svg.IndexOf("<svg");
                        if (svgStartIndex >= 0)
                        {
                            return svg.Substring(svgStartIndex);
                        }
                        return svg;
                    }
                    return $"<div class=\"alerta alerta-error\">Error al compilar Graphviz: {error}</div>";
                }
            }
            catch (Exception ex)
            {
                return $"<div class=\"alerta alerta-error\">No se pudo ejecutar la herramienta Graphviz (dot): {ex.Message}. Asegurese de tener Graphviz instalado y en la variable de entorno PATH.</div>";
            }
        }

        // FUNCION ENCARGADA DE REVISAR SI EL PROGRAMA GRAPHVIZ ESTA BIEN INSTALADA
        // RETORNANDO TRUE O FALSE DEPENDIENDO DEL RESULTADO
        private bool TestDotPath(string path)
        {
            try
            {
                var startInfo = new ProcessStartInfo
                {
                    FileName = path,
                    Arguments = "-V",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };
                using (var process = Process.Start(startInfo))
                {
                    process?.WaitForExit();
                    return process?.ExitCode == 0;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}