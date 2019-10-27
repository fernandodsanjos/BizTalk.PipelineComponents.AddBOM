# AddBOM - BizTalk PipelineComponent
This BizTalk pipeline component adds an encoding BOM to a unicode stream (utf-8 or utf-16).<br/>
## Background
When using the XmlTransmit you sometimes run in to the famous error 
`The document type "..." does not match any of the given schemas.`. The funny thing is that the message is already identified and also the encoding.   
You can try to change the docspec in the XMLTransmit pipeline or remove it, nothing helps.
## How
The component does not change the file encoding, it  only adds a BOM to the beginning of the stream.<br/>
BodyPart character set is used to add the correct BOM. For utf-16 LE is used.<br/>
## XML Declaration
The component also allows you to add a xml declaration, BodyPart character set is used to add the correct encoding.


