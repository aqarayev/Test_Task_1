using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace Inbank
{
    internal class Program
    {
        private const string InputFileName = "Test_task_1.csv";
        private const string OutputFolderPath = "./Test_Task_1_Result/";

        static void Main(string[] args)
        {
            string header, line, id, outputFileName;
            Dictionary<string, StreamWriter> outputFiles = new Dictionary<string, StreamWriter>();

            try
            {
                using (StreamReader inputFileReader = new StreamReader(InputFileName))
                {
                    header = inputFileReader.ReadLine();

                    while ((line = inputFileReader.ReadLine()) != null)
                    {
                        string[] columns = line.Split(";");

                        FormatDecimals(columns);

                        FormatDate(ref columns[4], "dd/MM/yyyy", "dd-MMM-yy");
                        FormatDate(ref columns[5], "dd/MM/yyyy", "yyyy-MM-dd");
                    
                        id = columns[1];

                        if (!outputFiles.TryGetValue(id, out StreamWriter outputFile))
                        {
                            outputFileName =  Path.Combine(OutputFolderPath, $"{id}.csv");
                            outputFiles[id] = new StreamWriter(outputFileName);
                            outputFiles[id].WriteLine(header);
                        }

                        outputFiles[id].WriteLine(string.Join(";", columns));
                    }
                }   
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }
            finally
            {
                foreach (StreamWriter outputFile in outputFiles.Values)
                {
                    outputFile.Dispose();
                }
            }
        }
        
        private static void FormatDecimals(string[] columns)
        {
            for (int i = 2; i <= 3; i++)
            {
                columns[i] = columns[i].Replace(',', '.');
            }
        }
        
        private static void FormatDate(ref string dateToChange, string initialDateFormat, string finalDateFormat)
        {
            if (DateTime.TryParseExact(
                    dateToChange, initialDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDateTime))
            {
                dateToChange = parsedDateTime.ToString(finalDateFormat);
            }
        }
    }
}