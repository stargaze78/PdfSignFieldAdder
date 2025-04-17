/*
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Affero General Public License as published
 * by the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 * GNU Affero General Public License for more details.
 *
 * You should have received a copy of the GNU Affero General Public License
 * along with this program. If not, see <https://www.gnu.org/licenses/>.
 *
 * Note: This tool uses iTextSharp (AGPL v3).
 */

using System;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace PdfSignFieldAdder
{
    /// <summary>
    /// Exit codes returned by the PdfSignFieldAdder tool to indicate the result of execution.
    /// </summary>
    enum ExitCode
    {
        Success = 0,                  // All operations completed successfully
        InvalidArguments = 1,         // Arguments are missing or malformed
        InputFileNotFound = 2,        // Specified input file does not exist
        OutputFileCreationFailed = 3, // Could not write to output file
        SignatureFieldError = 4,      // Error while adding the signature field
        UnknownError = 100            // Catch-all for unhandled errors
    }

    /// <summary>
    /// Main entry point for the PdfSignFieldAdder CLI tool.
    /// Adds a visible digital signature field to a PDF document.
    /// </summary>
    class Program
    {
        /// <summary>
        /// Main method that parses arguments, opens the PDF, and inserts a signature field.
        /// </summary>
        /// <param name="args">
        /// args[0] = input PDF path,
        /// args[1] = output PDF path,
        /// args[2] = field name,
        /// args[3-6] = rectangle coordinates (x1, y1, x2, y2)
        /// </param>
        static void Main(string[] args)
        {
            // Validate argument count
            if (args.Length < 7)
            {
                Console.Error.WriteLine("Usage: PdfSignFieldAdder.exe <input.pdf> <output.pdf> <fieldName> <x1> <y1> <x2> <y2>");
                Environment.Exit((int)ExitCode.InvalidArguments);
            }

            string inputPath = args[0];
            string outputPath = args[1];
            string fieldName = args[2];

            // Ensure input file exists
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                Environment.Exit((int)ExitCode.InputFileNotFound);
            }

            // Parse rectangle coordinates
            float x1, y1, x2, y2;
            try
            {
                x1 = float.Parse(args[3]);
                y1 = float.Parse(args[4]);
                x2 = float.Parse(args[5]);
                y2 = float.Parse(args[6]);
            }
            catch
            {
                Console.Error.WriteLine("Invalid rectangle coordinates.");
                Environment.Exit((int)ExitCode.InvalidArguments);
                return;
            }

            // Try to apply the signature field
            try
            {
                using (var reader = new PdfReader(inputPath))
                {
                    using (var fs = new FileStream(outputPath, FileMode.Create, FileAccess.Write))
                    {
                        using (var stamper = new PdfStamper(reader, fs))
                        {
                            var rect = new Rectangle(x1, y1, x2, y2);
                            var sigField = PdfFormField.CreateSignature(stamper.Writer);
                            sigField.FieldName = fieldName;
                            sigField.SetWidget(rect, PdfAnnotation.HIGHLIGHT_INVERT);
                            sigField.Flags = PdfAnnotation.FLAGS_PRINT;
                            sigField.AppearanceState = "Normal";
                            stamper.AddAnnotation(sigField, 1);
                        }
                    }
                }
            }
            catch (IOException ioEx)
            {
                Console.Error.WriteLine($"File IO Error: {ioEx.Message}");
                Environment.Exit((int)ExitCode.OutputFileCreationFailed);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unhandled Exception: {ex.Message}");
                Environment.Exit((int)ExitCode.SignatureFieldError);
            }

            // If we reach here, everything succeeded
            Environment.Exit((int)ExitCode.Success);
        }
    }
}