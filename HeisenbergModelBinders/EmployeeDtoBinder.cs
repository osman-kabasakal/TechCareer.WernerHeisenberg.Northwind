using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using TechCareer.WernerHeisenberg.Northwind.Dtos;

namespace TechCareer.WernerHeisenberg.Northwind.HeisenbergModelBinders;

public class EmployeeDtoBinder : IModelBinder
{
    public async Task BindModelAsync(ModelBindingContext bindingContext)
    {
        if (bindingContext == null)
        {
            throw new ArgumentNullException(nameof(bindingContext));
        }

        if (bindingContext.BindingSource == BindingSource.Body && bindingContext.HttpContext.Request.ContentLength>0)
        {
            var request = bindingContext.HttpContext.Request;
            request.EnableBuffering();
            var bufferMaxlength = int.MaxValue;
            var buffers= CreateBuffers(request, bufferMaxlength);

            var requestString = await ReadRequestAsString(buffers, request, bufferMaxlength);

            if (string.IsNullOrEmpty(requestString.Trim()))
            {
                return;
            }

            bindingContext.Result = ModelBindingResult.Success(JsonSerializer.Deserialize<EmployeeDto>(requestString));
            return;
        }

        var model = new EmployeeDto();
        
        var props= model.GetType().GetProperties().Where(x=>x.CanWrite).ToList();
        
        foreach (var prop in props)
        {
            var value = bindingContext.ValueProvider.GetValue(prop.Name);
            if (value == ValueProviderResult.None)
            {
                continue;
            }
            prop.SetValue(model, value.FirstValue);
        }
        
       bindingContext.Result= ModelBindingResult.Success(model);
       return;
    }

    private async Task<string> ReadRequestAsString(List<byte[]> buffers, HttpRequest request, int bufferMaxlength)
    {
        var requestString = string.Empty;
        foreach (var buffer in buffers)
        {
            var bufferIndex = buffers.IndexOf(buffer);
            await request.Body.ReadAsync(buffer, bufferMaxlength * bufferIndex, buffer.Length);
            requestString += Encoding.UTF8.GetString(buffer);
        }

        return requestString;
    }

    private  List<byte[]> CreateBuffers(HttpRequest request, int bufferMaxlength)
    {
        List<byte[]> buffers = new List<byte[]>();
        if (request.ContentLength <= bufferMaxlength)
        {
            buffers.Add(new byte[(int)request.ContentLength]);
        }
        else
        {
            var buffersLength = Math.Ceiling(decimal.Divide(request.ContentLength??0, bufferMaxlength));
            for (int i = 0; i < buffersLength; i++)
            {
                if ((i * bufferMaxlength) + bufferMaxlength > request.ContentLength)
                {
                    buffers.Add(new byte[request.ContentLength.Value - (i * bufferMaxlength)]);
                    break;
                }

                buffers.Add(new byte[bufferMaxlength]);
            }
        }

        return buffers;
    }
}