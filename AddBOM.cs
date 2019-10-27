using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using Microsoft.BizTalk.Component.Interop;
using Microsoft.BizTalk.Message.Interop;
using Microsoft.BizTalk.Streaming;
using IComponent = Microsoft.BizTalk.Component.Interop.IComponent;
using BizTalkComponents.Utils;
using System.Drawing.Design;
using System.Text;

namespace BizTalk.PipelineComponents.AddBOM
{
   

    [ComponentCategory(CategoryTypes.CATID_PipelineComponent)]
    [System.Runtime.InteropServices.Guid("f4a779b9-dfea-41a9-9327-f2d079d3abed")]
    [ComponentCategory(CategoryTypes.CATID_Any)]
    public partial class AddBOM : IComponent, IBaseComponent, IPersistPropertyBag, IComponentUI
    {
       [DisplayName("Add XmlDeclaration")]
       public bool AddXmlDeclaration { get; set; }

        public IBaseMessage Execute(IPipelineContext pContext, IBaseMessage pInMsg)
        {

            if(pInMsg.BodyPart != null)
            {
                string charset = pInMsg.BodyPart.Charset.ToUpper();

                if (charset == "UTF-8" || charset == "UTF-16")
                {
                    BOMStream stm = new BOMStream(pInMsg.BodyPart.Data, charset,this.AddXmlDeclaration);

                    pContext.ResourceTracker.AddResource(stm);

                    pInMsg.BodyPart.Data = stm;
                }
                
            }
           

            return pInMsg;
        }

        //Load and Save are generic, the functions create properties based on the components "public" "read/write" properties.
        public void Load(IPropertyBag propertyBag, int errorLog)
        {
            var props = this.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

            foreach (var prop in props)

            {

                if (prop.CanRead & prop.CanWrite)

                {

                    prop.SetValue(this, PropertyBagHelper.ReadPropertyBag(propertyBag, prop.Name, prop.GetValue(this)));

                }

            }


        }

        public void Save(IPropertyBag propertyBag, bool clearDirty, bool saveAllProperties)
        {
            var props = this.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

            foreach (var prop in props)

            {

                if (prop.CanRead & prop.CanWrite)

                {

                    PropertyBagHelper.WritePropertyBag(propertyBag, prop.Name, prop.GetValue(this));

                }

            }

        }
    }
}
