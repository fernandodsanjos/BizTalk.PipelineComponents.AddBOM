using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.BizTalk.Streaming;

namespace BizTalk.PipelineComponents.AddBOM
{
    public class BOMStream : System.IO.Stream
    {
        private System.Text.Encoding _sourceEncoding;
        private System.IO.Stream _stream;
        private bool _addXmlDeclaration;
        private string xmlDeclaration = "<?xml version=\"1.0\" encoding=\"{0}\"?>";
        

        public BOMStream(System.IO.Stream stream,string charset,bool addXmlDeclaration)
        {
            _stream = stream;
            _addXmlDeclaration = addXmlDeclaration;
            charset = charset.ToLower();

            xmlDeclaration = String.Format(xmlDeclaration, charset);

            

            if (charset == "utf-8")
            {
                _sourceEncoding = new UTF8Encoding(true);
            }
            else if (charset == "utf-16")
            {
                _sourceEncoding = new UnicodeEncoding(false, true);
            }
            else
                throw new ArgumentException("Charset {0} is not allowed, allowed charset's are utf-8 and utf-16", "BOMStream Constructor");
        }

        #region System.IO.Stream Overrides

        public override bool CanRead
        {
            get { return _stream.CanRead; }
        }

        public override bool CanSeek
        {
            get { return false; }
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override long Length
        {
            get { return _stream.Length + _sourceEncoding.GetPreamble().Length; }
        }

        public override long Position
        {
            get
            {
                return _stream.Position;
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        public override void SetLength(long value)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void Flush()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new Exception("The method or operation is not implemented.");
        }


        public override int Read(byte[] buffer, int offset, int count)
        {
            byte[] bom = _sourceEncoding.GetPreamble();
            
            int appendLen = bom.Length;

           

            //Remove BOM from source, by moving past BOM sequence
            if (_stream.Position == 0)
            {
                byte[] declaration = null;

                //<?xml version="1.0" encoding="{0}" standalone="no"?>
                if (_addXmlDeclaration == true)
                {
                    declaration = _sourceEncoding.GetBytes(xmlDeclaration);
                    appendLen = (appendLen + declaration.Length);
                }

                byte[] internalBuffer = new byte[(count - appendLen)];
                int read = _stream.Read(internalBuffer, offset, (count - appendLen));

                //Simple encoding check
                if (internalBuffer[0] != bom[0])
                {
                    bom.CopyTo(buffer, 0);

                    if (declaration != null)
                    {
                        declaration.CopyTo(buffer, bom.Length);
                    }

                }
                else
                {
                    //bom already exists
                    appendLen = (appendLen - bom.Length);

                    if (declaration != null)
                    {
                        declaration.CopyTo(buffer, 0);
                    }
                }

               
                internalBuffer.CopyTo(buffer, appendLen);

                return read + appendLen;
            }
            else
            {
                return _stream.Read(buffer, offset, count);
            }

        }

        #endregion

    }
}
