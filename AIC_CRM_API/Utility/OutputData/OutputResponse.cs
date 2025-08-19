using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility.OutputData
{
    public static class ResponseCode
    {
        public const int SAVE = 201; // HTTP 201 Created (successful creation)
        public const int CREATE = 201; // HTTP 201 Created (successful creation)
        public const int UPDATE = 200; // HTTP 200 OK (successful update)
        public const int DELETE = 204; // HTTP 204 No Content (successful deletion, no content returned)
        public const int GET = 200; // HTTP 200 OK (successful data retrieval)
        public const int GET_ALL = 200; // HTTP 200 OK (successful retrieval of all data)
        public const int GET_ALL_DROPDOWN = 200; // HTTP 200 OK (successful dropdown retrieval)
        public const int ALREADY_ACTIVE = 409; // HTTP 409 Conflict (already active)
        public const int ALREADY_INACTIVE = 409; // HTTP 409 Conflict (already inactive)
        public const int FOUND = 200; // HTTP 200 OK (found)
        public const int LOGIN_SUCCESS = 200; // HTTP 200 OK (found)
        public const int NOT_FOUND = 404; // HTTP 404 Not Found (resource not found)
        public const int EXIST = 409; // HTTP 409 Conflict (already exists)
        public const int NOT_EXIST = 404; // HTTP 404 Not Found (does not exist)
        public const int INVALID_REQUEST = 400; // HTTP 400 Bad Request (invalid request)
        public const int INVALID_DELETE = 400; // HTTP 400 Bad Request (invalid delete request)
        public const int BAD_REQUEST = 400; // HTTP 400 Bad Request

    }
    public static class ResponseMessage
    {
        public const string SAVE = "{0} is Save Successfully.";
        public const string CREATE = "{0} is Created Successfully.";
        public const string UPDATE = "{0} is Updated Successfully.";
        public const string DELETE = "{0} is Deleted Successfully.";
        public const string GET = "{0} Get Successfully.";
        public const string GET_ALL = "{0}s Get all Successfully.";
        public const string GET_ALL_DROPDOWN = "{0}s Get all Dropdown Successfully.";
        public const string ACTIVATED = "{0} is Activated Successfully";
        public const string DEACTIVATED = "{0} is De-Activated Successfully.";
        public const string ALREADY_ACTIVE = "{0} is Already Active.";
        public const string ALREADY_INACTIVE = "{0} is Already Inactive.";
        public const string EXIST = "{0} Already Exist.";
        public const string NOT_EXIST = "{0} does not Exist.";
        public const string INVALID_REQUEST = "Invalid Request";
        public const string SESSION_EXIST = "{0}";
        public const string CODE_NOT_EXIST = "{0} Code Not Exist";
        public const string BAD_REQUEST = "Bad Request";
        public const string LOGIN_SUCCESS = "Login Successfully";
    }

    public enum ResponseType
    {
        SAVE,
        CREATE,
        UPDATE,
        DELETE,
        GET,
        GET_ALL,
        GET_ALL_DROPDOWN,
        ACTIVATED,
        DEACTIVATED,
        ALREADY_INACTIVE,
        ALREADY_ACTIVE,
        EXIST,
        NOT_EXIST,
        SESSION_EXIST,
        INVALID_REQUEST,
        LOGIN_SUCCESS,
        CODE_NOT_EXIST,
        BAD_REQUEST
    }
}
