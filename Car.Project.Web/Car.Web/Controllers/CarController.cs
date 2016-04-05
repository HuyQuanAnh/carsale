using Carsale.Web.Authorization;
using Carsale.Web.Controllers;
using Carsale.Data.Interface;
using Carsale.Security.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Carsale.Web.Models;
using AutoMapper;
using Carsale.Data;
using System.IO;
using Carsale.Web.Helper;
using NLog;
using System.Text;

namespace Carsale.Web.Controllers
{
    public class CarController : AuthenticatedControllerBase
    {
        private ICarRepository _carRepository;

        public CarController(ICarRepository carRepository)
        {
            _carRepository = carRepository;
        }


        [AuthorizeUserRole(UserRole.Dealer, UserRole.User)]
        public ActionResult CarList()
        {
            var carDetailsList = _carRepository.GetCars(Server.MapPath("~/Data/CarCollection.json"));
            Mapper.CreateMap<CarDetails, CarDetailsModel>();
            var carDetailsModel = Mapper.Map<IList<CarDetails>, IList<CarDetailsModel>>(carDetailsList);
            return View(carDetailsModel);
        }
        [AuthorizeUserRole(UserRole.Dealer, UserRole.User)]
        public ActionResult CarDetails(string id)
        {
            if (string.IsNullOrEmpty(id))
                return View(new CarDetailsModel() { Id = (new Guid()).ToString() });
            var carDetails = _carRepository.GetCarById(id, Server.MapPath("~/Data/CarCollection.json")) as CarDetails;
            if (carDetails == null)
            {
                Logger logger = LogManager.GetCurrentClassLogger();
                logger.Error(string.Format("Car id {0} is not existing", id));
                return View("Error");
            }
            Mapper.CreateMap<CarDetails, CarDetailsModel>();
            var carDetailsModel = Mapper.Map<CarDetails, CarDetailsModel>(carDetails);
            var carDetailsEnquiry = new CardetailsEnquiry() { CarDetailsModel = carDetailsModel };
            return View(carDetailsEnquiry);
        }
        [HttpPost]
        [AuthorizeUserRole(UserRole.User)]
        public ActionResult EnquiryConfirmation(CardetailsEnquiry cardetailsEnquiry)
        {
            try
            {
                Mapper.CreateMap<EnquiryModel, EnquiryDetails>();
                var enquiryDetails = Mapper.Map<EnquiryModel, EnquiryDetails>(cardetailsEnquiry.EnquiryModel);
                enquiryDetails.CarId = cardetailsEnquiry.CarDetailsModel.Id;
                _carRepository.SaveEnquiry(enquiryDetails, Server.MapPath("~/Data/EnQuiryCollection.json"));
                // we might do not have STMP details as deploying to environemnt such as AWS
                ViewBag.EnableNotification = ConfigurationHelper.EnableNotification;
                if (ConfigurationHelper.EnableNotification)
                {                  
                    string subject = string.Format("Car Enquiry - Car ID is {0}", cardetailsEnquiry.CarDetailsModel.Id);
                    EmailHelper.SendEmail("noreply@carsale.com.au", cardetailsEnquiry.EnquiryModel.Email, subject, PopulateEmailBody(cardetailsEnquiry.EnquiryModel));
                }
                return View(cardetailsEnquiry);
            }
            catch (Exception ex)
            {
                Logger logger = LogManager.GetCurrentClassLogger();
                logger.ErrorException(ex.Message, ex);
                cardetailsEnquiry.ResponseDetails = new ResponseDetails { IsSuccessful = false, Message = ex.Message };
                return View(cardetailsEnquiry);
            }

        }
        [HttpPost]
        [AuthorizeUserRole(UserRole.Dealer)]
        public ActionResult Car(CarDetailsModel carDetailsModel)
        {
            string errorMsg = ValidateFile();
            if (!string.IsNullOrEmpty(errorMsg))
            {
                carDetailsModel.ResponseDetails = new ResponseDetails() { Message = errorMsg, IsSuccessful = false };
                return View(carDetailsModel);
            }
            string carId = string.IsNullOrEmpty(carDetailsModel.Id) ? (Guid.NewGuid()).ToString() : carDetailsModel.Id;
            var savedFileName = SaveFile(carId);
            Mapper.CreateMap<CarDetailsModel, CarDetails>();
            var carDetails = Mapper.Map<CarDetailsModel, CarDetails>(carDetailsModel);
            bool isSuccessful;
            if (string.IsNullOrEmpty(carDetailsModel.Id))
            {
                carDetails.Id = carId;
                carDetails.Photo = savedFileName;
                isSuccessful = _carRepository.AddCar(carDetails, Server.MapPath("~/Data/CarCollection.json"));
            }
            else
            {
                carDetails.Photo = string.IsNullOrEmpty(savedFileName) ? carDetails.Photo : savedFileName;
                isSuccessful = _carRepository.EditCar(carDetails, Server.MapPath("~/Data/CarCollection.json"));
            }
            if (!isSuccessful)
            {
                Logger logger = LogManager.GetCurrentClassLogger();
                logger.Error(string.Format("Failed to add/update the car - {0}", carDetailsModel.Id));
                carDetailsModel.ResponseDetails = new ResponseDetails() { Message = "Failed to add/update the car", IsSuccessful = false };
                return View(carDetailsModel);
            }
            // we should redirect to confirmation page. Let's go to Cars page for the purpose of this project`````                                                                                                                                                                                                                                                                                          
            return RedirectToAction("CarList");
        }
        [AuthorizeUserRole(UserRole.Dealer)]
        public ActionResult RemoveCar(string id)
        {
            var carDetails = _carRepository.GetCarById(id, Server.MapPath("~/Data/CarCollection.json")) as CarDetails;
            string errorMsg = ValiddateRemoveCar(id, carDetails);
            if (!string.IsNullOrEmpty(errorMsg))
            {
                Logger logger = LogManager.GetCurrentClassLogger();
                logger.Error(string.Format("{0} - car id {1}", errorMsg, id));
                return View("Error");
            }
            var isSucessful = _carRepository.RemoveCar(id, Server.MapPath("~/Data/CarCollection.json"));
            if (!isSucessful)
            {
                Logger logger = LogManager.GetCurrentClassLogger();
                logger.Error(string.Format("Failed to remove Car id {0}", id));
                return View("Error");
            }
            RemoveCarPhoto(carDetails.Photo);
            return RedirectToAction("CarList");
        }
        [AuthorizeUserRole(UserRole.Dealer)]
        public ActionResult Car(string id)
        {
            if (string.IsNullOrEmpty(id))
                return View(new CarDetailsModel());
            var carDetails = _carRepository.GetCarById(id, Server.MapPath("~/Data/CarCollection.json")) as CarDetails;
            if (carDetails == null)
                return View("Error");
            Mapper.CreateMap<CarDetails, CarDetailsModel>();
            var carDetailsModel = Mapper.Map<CarDetails, CarDetailsModel>(carDetails);
            return View(carDetailsModel);
        }
        [AuthorizeUserRole(UserRole.Dealer)]
        public ActionResult CarEnquiries(string id)
        {
            var carEnquiries = _carRepository.GetEnquiriesByCarId(id, Server.MapPath("~/Data/EnQuiryCollection.json"));
            Mapper.CreateMap<EnquiryDetails, EnquiryModel>();
            var carEnquiriesModel = Mapper.Map<IList<EnquiryDetails>, IList<EnquiryModel>>(carEnquiries);
            return View(carEnquiriesModel);
        }


        #region
        /// <summary>
        /// Save the file to the system foler
        /// </summary>
        /// <param name="filename">File name without the extention</param>
        private string SaveFile(string filename)
        {
            string savedFileName = string.Empty;
            if (Request.Files.Count > 0)
            {
                var file = Request.Files[0];
                if (file != null && file.ContentLength > 0)
                {
                    var fileExtention = Path.GetExtension(file.FileName);
                    savedFileName = string.Format("{0}{1}", filename, fileExtention);
                    var path = Path.Combine(Server.MapPath("~/CarImages/"), savedFileName);
                    file.SaveAs(path);
                }
            }
            return savedFileName;
        }
        private void RemoveCarPhoto(string filename)
        {
            var path = Path.Combine(Server.MapPath("~/CarImages/"), filename);
            if (System.IO.File.Exists(path))
                System.IO.File.Delete(path);

        }
        /// <summary>
        /// Validate the car image uploaded
        /// </summary>
        /// <returns>Empty string if the file id is valid. Otherwise, return the error message</returns>
        private string ValidateFile()
        {
            string errorMsg = string.Empty;
            if (Request.Files.Count > 0)
            {
                var file = Request.Files[0];
                if (file != null & file.ContentLength > 0)
                {
                    //we can use Regex here
                    List<string> fileTypes = new List<string>() { ".jpg", ".gif", ".png" };
                    var fileExtention = Path.GetExtension(file.FileName);
                    if (!fileTypes.Contains(fileExtention))
                        errorMsg = "The file is not valid. The valid files are .jpg and .png";
                }
            }
            return errorMsg;
        }
        /// <summary>
        /// Validate the given car id
        /// </summary>
        /// <param name="id">The car id</param>
        /// <returns>Empty string if the car id is valid. Otherwise, return the error message</returns>
        private string ValiddateRemoveCar(string id, CarDetails carDetails)
        {
            string errorMsg = string.Empty;
            if (string.IsNullOrEmpty(id))
                errorMsg = "The ID can not be empty.";
            if (carDetails == null)
                errorMsg = string.Format("The car id of {0} does not exist.", id);
            return errorMsg;
        }

        /// <summary>
        /// Populate the content email
        /// </summary>
        /// <param name="cardetailsEnquiry">The enquiry details</param>
        /// <returns></returns>content of email
        private string PopulateEmailBody(EnquiryModel enquiryModel)
        {
            string emailBody = System.IO.File.ReadAllText(Server.MapPath("~/EmailTemplate/EnquiryEmailTemplate.html"));
            StringBuilder sb = new StringBuilder(emailBody);
            sb.Replace("%Name%", enquiryModel.Name);
            sb.Replace("[ContactPhone]", enquiryModel.ContactPhone);
            sb.Replace("[Comment]", enquiryModel.Comment);
            return sb.ToString();
        }
        #endregion
    }
}
