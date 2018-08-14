using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GHC.Operations;
using System.Reflection;
using System.Data;
using System.Configuration;
using System.IO;
using Ghc.Utility;

namespace FixedWidthSerializerSample
{
    public class CentralProcessor
    {
        private CentralLogger log;
        private CentralEngine engine;
        private DateTime fileDate;
        private string controlNumber;
        private List<PriorAuthTestObject> priorAuthList;
        private List<ResponsePriorAuthTestObject> responsePriorAuthList;

        #region CONSTRUCTOR

        public CentralProcessor(CentralEngine inEngine, CentralLogger inLogger)
        {
            engine = inEngine;
            log = inLogger;
            fileDate = DateTime.Today;
            priorAuthList = new List<PriorAuthTestObject>();
        }

        #endregion

        #region METHOD: RunProcess(string startDate, string endDate)

        public void RunProcess()
        {
            log.WriteLine("*****************************************************************", eLogLevel.Informational);
            log.WriteLine("Start function - " + MethodBase.GetCurrentMethod().Name, eLogLevel.Informational);

            log.WriteLine("Process Output File - Create output file" + MethodBase.GetCurrentMethod().Name, eLogLevel.Informational);
            ProcessOutputFile();

            log.WriteLine("Process Response Incoming File - Read input file" + MethodBase.GetCurrentMethod().Name, eLogLevel.Informational);
            ProcessResponseFile();

            log.WriteLine("End function - " + MethodBase.GetCurrentMethod().Name, eLogLevel.Informational);
            log.WriteLine("*****************************************************************", eLogLevel.Informational);
        }

        #endregion

        #region METHOD: ProcessOutputFile()

        private void ProcessOutputFile()
        {
            string fileName = "TestFile_3.txt";
            StreamWriter writer = null;

            log.WriteLine("*****************************************************************", eLogLevel.Informational);
            log.WriteLine("Start function - " + MethodBase.GetCurrentMethod().Name, eLogLevel.Informational);

            try
            {
                GeneratePriorAuthorizationData();

                //Instantiate Steam Writer
                writer = new StreamWriter(engine.OutputPath + fileName);

                WriteHeaderRecord(writer);

                WritePriorAuthData(writer);

                WriteTrailerRecord(writer);

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (writer != null)
                {
                    writer.Close();
                }

                log.WriteLine("End function - " + MethodBase.GetCurrentMethod().Name, eLogLevel.Informational);
                log.WriteLine("*****************************************************************", eLogLevel.Informational);
            }
        }
        
        #endregion

        #region METHOD: GeneratePriorAuthorizationData(string startDate,string endDate)

        private void GeneratePriorAuthorizationData()
        {
            DataTable dt = null;
            log.WriteLine("*****************************************************************", eLogLevel.Informational);
            log.WriteLine("Start function - " + MethodBase.GetCurrentMethod().Name, eLogLevel.Informational);

            try
            {
                dt = GetData();
                
                log.WriteLine("Record Count: [" + dt.Rows.Count.ToString() + "]", eLogLevel.Informational);

                foreach (DataRow row in dt.Rows)
                {
                    PriorAuthTestObject pa = new PriorAuthTestObject();
                    pa = FixedWidthFileSerializer.CreateItem<PriorAuthTestObject>(row);
                    priorAuthList.Add(pa);
                }
            }
            catch (Exception ex)
            {
                log.WriteLine("Error ocurred: " + ex.Message + MethodBase.GetCurrentMethod().Name, eLogLevel.Error);
                throw ex;
            }

            log.WriteLine("End function - " + MethodBase.GetCurrentMethod().Name, eLogLevel.Informational);
            log.WriteLine("*****************************************************************", eLogLevel.Informational);
        }

        #endregion

        #region FUNCTION: GetData()

        private DataTable GetData()
        {
            try
            {
                // Here we create a DataTable with four columns.
                DataTable table = new DataTable();
                table.Columns.Add("RecordNumber", typeof(int));
                table.Columns.Add("MemberID", typeof(string));
                table.Columns.Add("BillingProviderID", typeof(string));
                table.Columns.Add("PriorAuthNumber", typeof(string));
                table.Columns.Add("DateReceived", typeof(DateTime));
                table.Columns.Add("DateFinalized", typeof(DateTime));
                table.Columns.Add("RenderingProviderID", typeof(int));
                table.Columns.Add("AuthorizedUnits", typeof(decimal));

                // Here we add five DataRows.
                table.Rows.Add(1, "11", "987", "5", DateTime.Now, DateTime.Now, 1, 42.12);
                table.Rows.Add(2, "22", "456", "4", DateTime.Now, DateTime.Now, 2, 38.47);
                table.Rows.Add(3, "33", "123", "3", DateTime.Now, DateTime.Now, 3, 279.18);
                table.Rows.Add(4, "44", "321", "2", DateTime.Now, DateTime.Now, 4, 45.62);
                table.Rows.Add(5, "55", "654", "1", DateTime.Now, DateTime.Now, 5, 101.52);

                return table;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region METHOD: WriteHeaderRecord(StreamWriter writer)

        private void WriteHeaderRecord(StreamWriter writer)
        {
            log.WriteLine("*****************************************************************", eLogLevel.Informational);
            log.WriteLine("Start function - " + MethodBase.GetCurrentMethod().Name, eLogLevel.Informational);

            try
            {
                Header header = new Header();
                header.CreationDate = DateTime.Today;
                header.CreationTime = DateTime.Now;

                FixedWidthFileSerializer.SerializeOutputFile(header, writer);

            }
            catch (Exception ex)
            {
                log.WriteLine("Error ocurred: " + ex.Message + MethodBase.GetCurrentMethod().Name, eLogLevel.Error);
                throw ex;
            }

            log.WriteLine("End function - " + MethodBase.GetCurrentMethod().Name, eLogLevel.Informational);
            log.WriteLine("*****************************************************************", eLogLevel.Informational);
        }

        #endregion

        #region METHOD: WritePriorAuthData(StreamWriter writer)

        private void WritePriorAuthData(StreamWriter writer)
        {
            log.WriteLine("*****************************************************************", eLogLevel.Informational);
            log.WriteLine("Start function - " + MethodBase.GetCurrentMethod().Name, eLogLevel.Informational);

            try
            {
                foreach (PriorAuthTestObject pa in priorAuthList)
                {
                    FixedWidthFileSerializer.SerializeOutputFile(pa, writer);
                }
            }
            catch (Exception)
            {

                throw;
            }

            log.WriteLine("End function - " + MethodBase.GetCurrentMethod().Name, eLogLevel.Informational);
            log.WriteLine("*****************************************************************", eLogLevel.Informational);
        }

        #endregion

        #region METHOD: WriteTrailerRecord(StreamWriter writer)

        private void WriteTrailerRecord(StreamWriter writer)
        {
            log.WriteLine("*****************************************************************", eLogLevel.Informational);
            log.WriteLine("Start function - " + MethodBase.GetCurrentMethod().Name, eLogLevel.Informational);

            try
            {

                Trailer trailer = new Trailer();
                trailer.DetailRecordCount = priorAuthList.Count;

                FixedWidthFileSerializer.SerializeOutputFile(trailer, writer);

                log.WriteLine("Trailer Record created successfully!!! Record Count: [" + priorAuthList.Count + "]. - " + MethodBase.GetCurrentMethod().Name, eLogLevel.Informational);

            }
            catch (Exception ex)
            {
                log.WriteLine("Error ocurred: " + ex.Message + MethodBase.GetCurrentMethod().Name, eLogLevel.Error);
                throw ex;
            }

            log.WriteLine("End function - " + MethodBase.GetCurrentMethod().Name, eLogLevel.Informational);
            log.WriteLine("*****************************************************************", eLogLevel.Informational);
        }

        #endregion

        #region FUNCTION: ProcessResponseFile()
        public void ProcessResponseFile()
        {
            try
            {
                log.WriteLine("*****************************************************************", eLogLevel.Informational);
                log.WriteLine("Start function - " + MethodBase.GetCurrentMethod().Name, eLogLevel.Informational);

                //Instantiate Object List 
                List<ResponsePriorAuthTestObject> responsePriorAuthList = new List<ResponsePriorAuthTestObject>();
                                
                //Deserialize data file to List of Object
                using (var stream = File.OpenRead(engine.OutputPath + "TestFile_3.txt"))
                {
                    responsePriorAuthList = Ghc.Utility.FixedWidthFileSerializer.DeserializeToList<ResponsePriorAuthTestObject>(stream);
                }
                
                //Convert List To DataTable
                DataTable dtPriorAuth = FixedWidthFileSerializer.ConvertTo<ResponsePriorAuthTestObject>(responsePriorAuthList);

                //Log Detail record
                log.WriteLine("RecordNumber" + "," + "MemberID" + "," + "BillingProviderID" + "," + "PriorAuthNumber" + "," + "DateReceived" + "," + "DateFinalized" + "," + "RenderingProviderID" + "," + "AuthorizedUnits", eLogLevel.Informational);

                foreach (var record in responsePriorAuthList)
                {
                    log.WriteLine(record.RecordNumber.ToString() + "," + record.MemberID + "," + record.BillingProviderID + "," + record.PriorAuthNumber + "," +
                                  record.DateReceived.ToShortDateString() + "," + record.DateFinalized.ToShortDateString() + "," + record.RenderingProviderID.ToString() + "," +
                                  record.AuthorizedUnits.ToString(), eLogLevel.Informational);
                }
            }
            catch (Exception ex)
            {
                log.WriteLine("Error ocurred: " + ex.Message, eLogLevel.Error);
                throw;
            }

            finally
            {
                log.WriteLine("End function - " + MethodBase.GetCurrentMethod().Name, eLogLevel.Informational);
                log.WriteLine("*****************************************************************", eLogLevel.Informational);
            }
        }
        #endregion

    }
}
