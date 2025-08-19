using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Utility.OutputData
{
    public static class OutputHandler
    {
        public static OutputDTO<T> Handler<T>(int responseCode, T dto, string name = "Record", int totalCount = 0)
        {
            var obj = new OutputDTO<T>()
            {
                Data = dto,
                TotalCounts = totalCount,
            };

            switch (responseCode)
            {
                case (int)ResponseType.SAVE:
                    obj.HttpStatusCode = ResponseCode.SAVE;
                    obj.Message = string.Format(ResponseMessage.SAVE, name);
                    break;

                case (int)ResponseType.CREATE:
                    obj.HttpStatusCode = ResponseCode.CREATE;
                    obj.Message = string.Format(ResponseMessage.CREATE, name);
                    break;

                case (int)ResponseType.UPDATE:
                    obj.HttpStatusCode = ResponseCode.UPDATE;
                    obj.Message = string.Format(ResponseMessage.UPDATE, name);
                    break;

                case (int)ResponseType.DELETE:
                    obj.HttpStatusCode = ResponseCode.DELETE;
                    obj.Message = string.Format(ResponseMessage.DELETE, name);
                    break;

                case (int)ResponseType.GET:
                    obj.HttpStatusCode = ResponseCode.GET;
                    obj.Message = string.Format(ResponseMessage.GET, name);
                    break;

                case (int)ResponseType.GET_ALL:
                    obj.Succeeded = true;
                    obj.HttpStatusCode = ResponseCode.GET_ALL;
                    obj.Message = string.Format(ResponseMessage.GET_ALL, name);
                    break;

                case (int)ResponseType.GET_ALL_DROPDOWN:
                    obj.HttpStatusCode = ResponseCode.GET_ALL_DROPDOWN;
                    obj.Message = string.Format(ResponseMessage.GET_ALL_DROPDOWN, name);
                    break;

                case (int)ResponseType.ACTIVATED:
                    obj.HttpStatusCode = (int)HttpStatusCode.OK;
                    obj.Message = string.Format(ResponseMessage.ACTIVATED, name);
                    break;

                case (int)ResponseType.DEACTIVATED:
                    obj.HttpStatusCode = (int)HttpStatusCode.OK;
                    obj.Message = string.Format(ResponseMessage.DEACTIVATED, name);
                    break;

                case (int)ResponseType.ALREADY_ACTIVE:
                    obj.Succeeded = false;
                    obj.HttpStatusCode = (int)HttpStatusCode.OK;
                    obj.Message = string.Format(ResponseMessage.ALREADY_ACTIVE, name);
                    break;

                case (int)ResponseType.ALREADY_INACTIVE:
                    obj.Succeeded = false;
                    obj.HttpStatusCode = ResponseCode.ALREADY_INACTIVE;
                    obj.Message = string.Format(ResponseMessage.ALREADY_INACTIVE, name);
                    break;

                case (int)ResponseType.EXIST:
                    obj.Succeeded = false;
                    obj.HttpStatusCode = ResponseCode.EXIST;
                    obj.Message = string.Format(ResponseMessage.EXIST, name);
                    break;

                case (int)ResponseType.NOT_EXIST:
                    obj.Succeeded = false;
                    obj.HttpStatusCode = ResponseCode.NOT_EXIST;
                    obj.Message = string.Format(ResponseMessage.NOT_EXIST, name);
                    break;

                case (int)ResponseType.SESSION_EXIST:
                    obj.Succeeded = false;
                    obj.HttpStatusCode = ResponseCode.NOT_EXIST;
                    obj.Message = string.Format(ResponseMessage.SESSION_EXIST, name);
                    break;

                case (int)ResponseType.INVALID_REQUEST:
                    obj.Succeeded = false;
                    obj.HttpStatusCode = ResponseCode.INVALID_REQUEST;
                    obj.Message = ResponseMessage.INVALID_REQUEST;
                    break;

                case (int)ResponseType.LOGIN_SUCCESS:
                    obj.Succeeded = true;
                    obj.HttpStatusCode = ResponseCode.LOGIN_SUCCESS;
                    obj.Message = ResponseMessage.LOGIN_SUCCESS;
                    break;

                case (int)ResponseType.CODE_NOT_EXIST:
                    obj.Succeeded = false;
                    obj.HttpStatusCode = ResponseCode.NOT_EXIST;
                    obj.Message = string.Format(ResponseMessage.CODE_NOT_EXIST, name);
                    break;
            }

            return obj;
        }
    }
}
