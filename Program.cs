﻿using System;
using System.IO;
using System.Linq;

namespace Charts {

    public static class Program {

        public static string currentTicker = string.Empty;

        private static Config config;
        private static string dirPath;

        static void Main (string [] args) {
            try {
                if (args.Count() <= 1) {
                    Utils.WriteToConsole ("Usage: quick-chart.exe [ticker] [float]\n\n\tArguments missing.", true, "Main");
                    Environment.Exit (1);
                }

                config = new Config ();
                dirPath = config.Get ("dirpath");
                if (dirPath == null || dirPath == string.Empty) {
                    Utils.WriteToConsole ("Setting 'dirpath' missing from App.config.", true, "Main");
                    Environment.Exit (2);
                }

                // ProcessTicker (tickerString, floatString);
                ProcessTicker (args [0], args[1]);
            }
            catch (Exception ex) {
                Utils.WriteToConsole (Utils.ExToString (ex), true, "Main");
            }
        }


        private static bool IsValidDirectory (this string dirPath) {
            try {
                if (Directory.Exists (dirPath)) {
                    return true;
                }
                else
                    throw new Exception ($"{dirPath} does not exist.");
            }
            catch (Exception ex) {
                Utils.WriteToConsole (Utils.ExToString (ex), true, "IsValidDirectory");
                return false;
            }
        }

        private static DailyStockList OutputDailyStockData (DailyStockList list, string tickers, string filePath) {
            try {
                using StreamWriter file = new StreamWriter (filePath);
                file.WriteLine (DailyStockData.Header);
                foreach (DailyStockData value in list) {
                    file.WriteLine (value.ToCsv ());
                }
            }
            catch (Exception ex) {
                Utils.WriteToConsole (Utils.ExToString (ex), true, "OutputDailyStockData");
            }
            return list;
        }

        private static void ProcessTicker (string ticker, string floatStr) {
            currentTicker = ticker;
            var flt = Convert.ToInt64 (floatStr);
            if (dirPath.IsValidDirectory ()) {
                string outputFile = $"{dirPath}{ticker}.csv";
                string floatFile = $"{dirPath}{ticker}-float.csv";
                string pointFile = $"{dirPath}{ticker}-point.csv";

                var result = DailyStockList.GetData (ticker);
                if (result.IsSuccess) {
                    var list = OutputDailyStockData (result.Success, ticker, outputFile);

                    var floats = new DailyFloatList (list, flt);
                    floats.OutputAsCsv (floatFile);

                    var points = new DailyPointList (list);
                    points.OutputAsCsv (pointFile);
                }
                else {
                    Utils.WriteToConsole (result.Failure.Item1, true, result.Failure.Item2);
                }
            }
            // No reason for an 'else' as the exception has already been recorded.
        }
    }
}
