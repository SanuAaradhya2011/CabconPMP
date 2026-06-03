using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace IntegratedCalibration.Communication
{
    public class OTAImageEntity
    {
        public byte Version { get; set; } //--1Byte

        public byte[] Reservedbyte { get; set; } //--4Byte

        public byte[] ReservedForUtility { get; set; } //--2Byte

        public byte[] Size { get; set; } //--4Byte

        public byte[] ImageSize { get; set; } //--4Byte

        public byte[] ImageCRC { get; set; } //--4Byte //--Incase of TPDDL

        public byte FileType { get; set; } //0-->Plain 1--> Encrypted  //--1Byte

        public byte[] ManufacturerCode { get; set; } //--3byte

        public byte[] MeterModel { get; set; } //--10 byte

        public byte MeterPhaseWireType { get; set; } //--1Byte

        public byte[] FirmwareVersion { get; set; } //--8 byte

        public byte SizeOfImageIdentifier { get; set; } //--1Byte

        public byte[] ImageIdentifier { get; set; } //--33 byte
        public List<List<byte>> FirmwareImagelists = new List<List<byte>>();

        public OTAImageEntity()
        {
            Version = 0x00;
            Reservedbyte = new byte[4];
            ReservedForUtility = new byte[2];
            Size = new byte[4];
            ImageCRC = new byte[4];
            FileType = 0x00;
            ManufacturerCode = new byte[3];
            MeterModel = new byte[10];
            MeterPhaseWireType = 0x00;
            FirmwareVersion = new byte[8];
            SizeOfImageIdentifier = 0x00;
            ImageIdentifier = new byte[33];
        }

        public enum ImageBlockAttribute : byte
        {
            image_block_size = 0x02,
            image_transferred_blocks_status = 0x03,
            image_first_not_transferred_block_number = 0x04,
            image_transfer_enabled = 0x05,
            image_transfer_status = 0x06,
            image_to_activate_info = 0x07,
        };
        public enum ImageBlockMethod : byte
        {
            image_transfer_initiate = 0x01,
            image_block_transfer = 0x02,
            image_verify = 0x03,
            image_activate_enabled = 0x04,
        };
        public enum ImageTransferStatus : byte
        {
            Image_transfer_not_initiated = 0x00,
            Image_transfer_initiated = 0x01,
            Image_verification_initiated = 0x02,
            Image_verification_successful = 0x03,
            Image_verification_failed = 0x04,
            Image_activation_initiated = 0x05,
            Image_activation_successful = 0x06,
            Image_activation_failed = 0x07
        };
        public enum ImageTransferEnabeling : byte
        {
            Disable = 0x00,
            Enable = 0x01
        };

        public OTAImageEntity ParseImageFile(string datastructure)
        {
            OTAImageEntity objimageEntity = new OTAImageEntity();
            try
            {
                int startArrayIndex;
                string[] refData = datastructure.Split(',');
                if (!int.TryParse(refData[0], out startArrayIndex)) startArrayIndex = 45; // Fixed as per 3rd party spec pdf if not valid input
                if (!File.Exists(datastructure))
                {
                    return null;
                }
                byte[] Tempinputfiledata = File.ReadAllBytes(datastructure);
                byte[] ImageHeader = new byte[startArrayIndex];
                byte[] FirmwareImage = new byte[(Tempinputfiledata.Length) - (startArrayIndex)];
                Array.Copy(Tempinputfiledata, 0, ImageHeader, 0, ImageHeader.Length);
                Array.Copy(Tempinputfiledata, startArrayIndex, FirmwareImage, 0, Tempinputfiledata.Length - (startArrayIndex));
                string strfiledata = string.Join(string.Empty, Array.ConvertAll(FirmwareImage, b => b.ToString("X2")));
                int startHeaderIndex = 0;
                objimageEntity.Version = ImageHeader[startHeaderIndex++];
                objimageEntity.Reservedbyte = new byte[1];
                Array.Copy(ImageHeader, startHeaderIndex, objimageEntity.Reservedbyte, 0, objimageEntity.Reservedbyte.Length); // file Header Constant
                startHeaderIndex += objimageEntity.Reservedbyte.Length;
                Array.Copy(ImageHeader, startHeaderIndex, objimageEntity.Size, 0, objimageEntity.Size.Length);
                startHeaderIndex += objimageEntity.Size.Length;
                Array.Copy(ImageHeader, startHeaderIndex, objimageEntity.ImageCRC, 0, objimageEntity.Size.Length);
                startHeaderIndex += objimageEntity.ImageCRC.Length;
                objimageEntity.SizeOfImageIdentifier = (byte)(startArrayIndex - startHeaderIndex);
                objimageEntity.ImageIdentifier = new byte[objimageEntity.SizeOfImageIdentifier];
                Array.Copy(ImageHeader, startHeaderIndex, objimageEntity.ImageIdentifier, 0, objimageEntity.ImageIdentifier.Length);
                startHeaderIndex += objimageEntity.ImageIdentifier.Length;
                if (FirmwareImage.Length <= 0)
                {
                    //testresult.Remarks = "Invalid Image Header";
                    //logger.Write("Invalid Image Header" + GlobalMessageTable.GlobalMessageList[GlobalMessageTable.MessageID.FAIL]);
                    //testresult.IsSuccess = false;
                    return null;
                }

                objimageEntity.ImageSize = BitConverter.GetBytes(FirmwareImage.Length).Reverse().ToArray();
                string imageBlockSize = "256";
                objimageEntity.FirmwareImagelists.Clear();
                List<List<byte>> firmwareImagePacket = new List<List<byte>>();
                var lists = new List<List<byte>>(int.Parse(imageBlockSize));
                for (int byteCounts = 0; byteCounts < FirmwareImage.Length; byteCounts += int.Parse(imageBlockSize))
                {
                    var templist = new List<byte>();
                    if (byteCounts + int.Parse(imageBlockSize) <= FirmwareImage.Length) templist.AddRange(FirmwareImage.ToList().GetRange(byteCounts, int.Parse(imageBlockSize)));
                    else templist.AddRange(FirmwareImage.ToList().GetRange(byteCounts, FirmwareImage.Length - byteCounts));
                    //firmwareImagePacket.Add(templist);
                    //lists.Add(templist);
                    objimageEntity.FirmwareImagelists.Add(templist);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            return objimageEntity;
        }
    }
}
