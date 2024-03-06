using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace UPrinceV4.Web.Models;

public class ApiBadRequestResponse : ApiResponse
{
    //public string Message { get; }
    private ModelStateDictionary _modelState;

    public ApiBadRequestResponse(ModelStateDictionary modelState)
        : base(400, false)
    {
        if (modelState != null && modelState.IsValid)
            throw new ArgumentException("ModelState must be invalid", nameof(modelState));

        //Message = modelState.SelectMany(x => x.Value.Errors)
        //    .Select(x => x.ErrorMessage).ToString();
        Message = "";
        if (modelState != null)
            foreach (var error in modelState.SelectMany(x => x.Value.Errors))
                Message += error.ErrorMessage + " ";
    }

    public ModelStateDictionary ModelState
    {
        set
        {
            _modelState = value;
            if (_modelState != null && _modelState.IsValid)
                throw new ArgumentException("ModelState must be invalid", nameof(_modelState));

            Message = "";
            if (_modelState != null)
                foreach (var error in _modelState.SelectMany(x => x.Value.Errors))
                    Message += error.ErrorMessage + " ";
        }
    }
}