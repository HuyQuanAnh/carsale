using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Carsale.Data.Interface
{
    public interface ICarRepository
    {
        #region car
        /// <summary>
        /// Get list of cars from XML file
        /// </summary>
        /// <param name="filePath">Full file path</param>
        /// <returns>List of cars</returns>
        IList<CarDetails> GetCars(string carCollectionFile);
        /// <summary>
        /// Get car details by id
        /// </summary>
        /// <param name="id"> car id</param>
        ///  /// <param name="filePath">Full file path</param>
        /// <returns>Car details</returns>
        ICarDetails GetCarById(string id, string carCollectionFile);      
        /// <summary>
        /// Add the new car
        /// </summary>
        /// <param name="carDetails">Details of the car</param>
        /// <returns>True if successful, otherwise false</returns>
        bool AddCar(ICarDetails carDetails, string carCollectionFile);
        /// <summary>
        /// Add the new car
        /// </summary>
        /// <param name="carDetails">Details of the car</param>
        /// <returns>True if successful, otherwise false</returns>
        bool EditCar(ICarDetails carDetails, string carCollectionFile);
        /// <summary>
        /// Remove the car
        /// </summary>
        /// <param name="Id">the Car Id</param>
        /// <param name="filePath">Full file path</param>
        /// <returns>True if successful, otherwise false</returns>
        bool RemoveCar(string Id, string carCollectionFile);
        #endregion
        #region Enquiry
        /// <summary>
        /// Save the Enquiry details
        /// </summary>
        /// <param name="enquiryDetails">The enquiry details</param>
        /// <param name="filePath">Full file path</param>
        /// <returns>True if successful, otherwise false</returns>
        bool SaveEnquiry(IEnquiryDetails enquiryDetails, string carCollectionFile);
        /// <summary>
        /// Get Enquries by car Id
        /// </summary>
        /// <param name="carId">Car id</param>
        /// <returns>List of enquiries</returns>
        IList<EnquiryDetails> GetEnquiriesByCarId(string carId, string enquiryCollectionFile);
        #endregion
    }
}
