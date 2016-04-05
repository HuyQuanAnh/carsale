using Carsale.Data.Interface;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Carsale.Data
{
    public class CarRepository : ICarRepository
    {
        #region Cars
        /// <summary>
        /// Get all the cars
        /// </summary>
        /// <returns>List of cars</returns>
        public IList<CarDetails> GetCars(string carCollectionFile)
        {
            return GetCarList(carCollectionFile);
        }
        public ICarDetails GetCarById(string id, string carCollectionFile)
        {
            var cars = GetCarList(carCollectionFile);
            return cars.FirstOrDefault(c => string.Equals(c.Id, id, StringComparison.CurrentCultureIgnoreCase));
        }
        /// <summary>
        /// Add the new car
        /// </summary>
        /// <param name="carDetails">Details of the car</param>
        /// <returns>True if successful, otherwise false</returns>
        public bool AddCar(ICarDetails carDetails, string carCollectionFile)
        {
            try
            {
                var cars = GetCarList(carCollectionFile);
                cars.Add(carDetails as CarDetails);
                SaveToJsFile(cars, carCollectionFile);
                return true;
            }
            catch (Exception ex)
            {
                Logger logger = LogManager.GetCurrentClassLogger();
                logger.Error("Failed to add the new car", ex);
                return false;
            }
        }
        /// <summary>
        /// Add the new car
        /// </summary>
        /// <param name="carDetails">Details of the car</param>
        /// <returns>True if successful, otherwise false</returns>
        public bool EditCar(ICarDetails carDetails, string carCollectionFile)
        {
            try
            {
                var updatedCarDetails = carDetails as CarDetails;
                var cars = GetCarList(carCollectionFile);
                // remove and add the new details for now
                // we can use sp as using sql database
                var removingItem = cars.FirstOrDefault(c => string.Equals(c.Id, updatedCarDetails.Id, StringComparison.CurrentCultureIgnoreCase));
                cars.Remove(removingItem);
                cars.Add(updatedCarDetails);
                SaveToJsFile(cars, carCollectionFile);
                return true;
            }
            catch (Exception ex)
            {
                Logger logger = LogManager.GetCurrentClassLogger();
                logger.Error(string.Format("Failed to edit the car details"), ex);
                return false;
            }

        }
        /// <summary>
        /// Remove the car
        /// </summary>
        /// <param name="Id">the Car Id</param>
        /// ///  /// <param name="filePath">Full file path</param>
        /// <returns>True if successful, otherwise false</returns>
        public bool RemoveCar(string id, string carCollectionFile)
        {
            try
            {

                var cars = GetCarList(carCollectionFile);
                var removingItem = cars.FirstOrDefault(c => string.Equals(c.Id, id, StringComparison.CurrentCultureIgnoreCase));
                if (removingItem == null)
                    throw new ApplicationException(string.Format("The car id of {0} does not exist.", id));
                cars.Remove(removingItem);
                SaveToJsFile(cars, carCollectionFile);
                return true;
            }
            catch (Exception ex)
            {
                Logger logger = LogManager.GetCurrentClassLogger();
                logger.Error(string.Format("Failed to remove the car - car id {0}",id), ex);
                return false;
            }
        }
        /// <summary>
        /// Get list of cars from XML file
        /// </summary>
        /// <param name="carCollectionFile">Full file path</param>
        /// <returns>List of cars</returns>
        private List<CarDetails> GetCarList(string carCollectionFile)
        {
            using (StreamReader file = File.OpenText(carCollectionFile))
            {
                JsonSerializer serializer = new JsonSerializer();
                List<CarDetails> cars = (List<CarDetails>)serializer.Deserialize(file, typeof(List<CarDetails>));
                return cars ?? new List<CarDetails>(); ;
            }
        }
        /// <summary>
        /// Save the cars to xml file
        /// </summary>
        /// <param name="cars">List of cars</param>
        /// <param name="carCollectionFile">Full XML file name</param>       
        private void SaveToJsFile(List<CarDetails> cars, string carCollectionFile)
        {
            using (StreamWriter file = File.CreateText(carCollectionFile))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, cars);
            }
        }
        #endregion
        #region Enquiry
        /// <summary>
        /// Save the Enquiry details
        /// </summary>
        /// <param name="enquiryDetails">The enquiry details</param>
        /// <param name="filePath">Full file path</param>
        /// <returns>True if successful, otherwise false</returns>
        public bool SaveEnquiry(IEnquiryDetails enquiryDetails, string enquiryCollectionFile)
        {
            var enquiries = GetEnquiryList(enquiryCollectionFile);
            enquiries.Add(enquiryDetails as EnquiryDetails);
            SaveToXmlFile(enquiries, enquiryCollectionFile);
            return true;
        }
        /// <summary>
        /// Get Enquries by car Id
        /// </summary>
        /// <param name="carId">Car id</param>
        /// <returns>List of enquiries</returns>
        public IList<EnquiryDetails> GetEnquiriesByCarId(string carId, string enquiryCollectionFile)
        {
            var allEnquiries = GetEnquiryList(enquiryCollectionFile).Where(e => string.Equals(e.CarId, carId, StringComparison.CurrentCultureIgnoreCase)).ToList();
            return allEnquiries as IList<EnquiryDetails>;
        }
        /// <summary>
        /// Get list of enquires
        /// </summary>
        /// <param name="filePath">Full file path</param>
        /// <returns>List of Enquiries</returns>
        private List<EnquiryDetails> GetEnquiryList(string enquiryCollectionFile)
        {
            using (StreamReader file = File.OpenText(enquiryCollectionFile))
            {
                JsonSerializer serializer = new JsonSerializer();
                List<EnquiryDetails> enquiryDetailsList = (List<EnquiryDetails>)serializer.Deserialize(file, typeof(List<EnquiryDetails>));
                return enquiryDetailsList ?? new List<EnquiryDetails>();
            }
        }
        /// <summary>
        /// Save enquiry
        /// </summary>
        /// <param name="filePath">Full file path</param>
        /// <param name="enquiryCollectionFile">List of Enquiries</param>
        private void SaveToXmlFile(List<EnquiryDetails> enquiries, string enquiryCollectionFile)
        {            
            using (StreamWriter file = File.CreateText(enquiryCollectionFile))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, enquiries);
            }
        }
        #endregion}
    }
}
