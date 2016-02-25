using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using Cereal64.Microcodes.F3DZEX.DataElements;

namespace TextureEditor
{
    public struct FileTableEntry
    {
        public int StartOffset;
        public short Unknown1;
        public short CompressedSize;
        public short Unknown2;
        public short DecompressedSize;

        public byte[] ToBytes()
        {
            byte[] output = new byte[12];
            byte[] temp = BitConverter.GetBytes(StartOffset);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(temp);
            Array.Copy(temp, 0, output, 0, 4);

            temp = BitConverter.GetBytes(Unknown1);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(temp);
            Array.Copy(temp, 0, output, 4, 2);

            temp = BitConverter.GetBytes(CompressedSize);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(temp);
            Array.Copy(temp, 0, output, 6, 2);

            temp = BitConverter.GetBytes(Unknown2);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(temp);
            Array.Copy(temp, 0, output, 8, 2);

            temp = BitConverter.GetBytes(DecompressedSize);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(temp);
            Array.Copy(temp, 0, output, 10, 2);

            return output;
        }

        public FileTableEntry(byte[] data) : this()
        {
            if (data.Length != 12)
                return;
            
            byte[] intData = new byte[4];
            Array.Copy(data, 0, intData, 0, 4);

            if (BitConverter.IsLittleEndian == true)
                Array.Reverse(intData);

            StartOffset = BitConverter.ToInt32(intData, 0);

            byte[] shortData = new byte[2];
            Array.Copy(data, 4, shortData, 0, 2);
            if (BitConverter.IsLittleEndian == true)
                Array.Reverse(shortData);
            Unknown1 = BitConverter.ToInt16(shortData, 0);

            Array.Copy(data, 6, shortData, 0, 2);
            if (BitConverter.IsLittleEndian == true)
                Array.Reverse(shortData);
            CompressedSize = BitConverter.ToInt16(shortData, 0);

            Array.Copy(data, 8, shortData, 0, 2);
            if (BitConverter.IsLittleEndian == true)
                Array.Reverse(shortData);
            Unknown2 = BitConverter.ToInt16(shortData, 0);

            Array.Copy(data, 10, shortData, 0, 2);
            if (BitConverter.IsLittleEndian == true)
                Array.Reverse(shortData);
            DecompressedSize = BitConverter.ToInt16(shortData, 0);
        }
    }

    public partial class mainForm : Form
    {
        private List<byte> _romData;
        private List<byte> _decompressedLevelData;
        private int _lastFileEntryIndex;

        private int[] HardcodedReferences = { 0x78AE0, 0x78AD8, 0x78AD4, 0x78AD0, 0x9EED0,
            0x9EED4, 0x9EED8, 0x9EEDC, 0xA2EF0, 0xA2EF4, 0xA2EF8, 0x9C220, 0xA2EFC, 0x9C224, 0x9C228,
            0x9C22C, 0xE89B8, 0xE89B0, 0xE89AC, 0x81EBC, 0xE89A8, 0x81EB4, 0x81EB0, 0x81EAC, 0x86300,
            0x862F8, 0x862F4, 0x8443C, 0x862F0, 0x84434, 0x84430, 0x8442C, 0x1194C4, 0x1194C0, 0x1194BC,
            0x3D768, 0x1194B8, 0x3D75C, 0x3D760, 0x3D764, 0x3D750, 0x3D754, 0x3D758, 0x3D78C, 0x3D790, 0x3D794,
            0x3D798, 0x3D79C, 0x3D7A0 };

        private enum LevelNames
        {
            HyruleTemple = 0
        }

        private int[] LevelFileTableOffset = { 113 };

        List<FileTableEntry> FileTable = new List<FileTableEntry>();

        private LevelNames SelectedLevel;

        public mainForm()
        {
            InitializeComponent();

            cbLevel.SelectedIndex = 0;
        }

        private void btnLoadRom_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                _romData = new List<byte>();
                _romData.AddRange(File.ReadAllBytes(openFileDialog.FileName));

                byte[] fileEnd = new byte[4];
                Array.Copy(_romData.ToArray(), 0x3D7A0, fileEnd, 0, 4);
                if(BitConverter.IsLittleEndian)
                    Array.Reverse(fileEnd);
                int fileEndOffset = BitConverter.ToInt32(fileEnd, 0);
                for (; _romData.Count > fileEndOffset; )
                {
                    _romData.RemoveAt(_romData.Count - 1);
                }

                FileTable = new List<FileTableEntry>();

                byte[] fileTableData = new byte[12];

                byte[] romArray = _romData.ToArray();


                _lastFileEntryIndex = 0;

                //Load the file table
                for (int i = 0x1AC870; i < 0x1B2C6C; i += 12)
                {
                    Array.Copy(romArray, i, fileTableData, 0, 12);
                    FileTable.Add(new FileTableEntry(fileTableData));
                    if (i == 0x1AC870 + 112 * 12)
                        Array.Copy(romArray, i, fileTableData, 0, 12);
                    if (FileTable.Last().StartOffset + 4 * FileTable.Last().CompressedSize + 0x1B2C6C > _lastFileEntryIndex)
                        _lastFileEntryIndex = FileTable.Last().StartOffset + 4 * FileTable.Last().CompressedSize + 0x1B2C6C;
                }

                btnLoadLevel.Enabled = true;
                btnSaveRom.Enabled = true;
                cbLevel.Enabled = true;
            }
        }

        private void btnSaveRom_Click(object sender, EventArgs e)
        {
            if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                int romSize = (_romData.Count / 0x400000) * 0x400000;
                if (_romData.Count - romSize > 0)
                    romSize += 0x400000;

                //Now the extra juicy bit, find out if we need to pad the end of the filetable
                FileTableEntry lastEntry = FileTable[0];
                foreach (FileTableEntry entry in FileTable)
                {
                    if ((entry.StartOffset & 0x7FFFFFFF) > (lastEntry.StartOffset & 0x7FFFFFFF))
                        lastEntry = entry;
                }

                int lastEntryEnd = (lastEntry.StartOffset & 0x7FFFFFFF) + lastEntry.CompressedSize * 4 + 0x1B2C6C;

                //int fileTableSize = (FileTable.Sum(x => x.CompressedSize));//lastEntryEnd - (FileTable.Min(x => x.StartOffset & 0x7FFFFFFF) & 0x7FFFFFFF);
                int extraData = (lastEntryEnd - _lastFileEntryIndex) % 16;
                if (extraData < 0)
                    extraData += 16;
                int padLength = 0;
                if (extraData > 0)
                    padLength = 16 - extraData;
                byte[] padVals = { 0xB6, 0x55, 0x23, 0x95, 0x30, 0xEC, 0x2B, 0x8D, 0xB6, 0x55, 0x23, 0x95, 0x30, 0xEC, 0x2B, 0x8D };

                romSize += padLength;

                byte[] romArray = new byte[romSize];

                Array.Copy(_romData.ToArray(), 0, romArray, 0, lastEntryEnd);
                for (int i = 0; i < padLength; i++)
                {
                    romArray[i + lastEntryEnd] = padVals[extraData + i];
                }
                Array.Copy(_romData.ToArray(), lastEntryEnd, romArray, lastEntryEnd + padLength, _romData.Count - lastEntryEnd);
                
                for (int i = _romData.Count + padLength; i < romSize; i++)
                    romArray[i] = (byte)0xFF;

                for (int i = 0; i < FileTable.Count; i++)
                {
                    byte[] fileEntryBytes = FileTable[i].ToBytes();
                    Array.Copy(fileEntryBytes, 0, romArray, 0x1AC870 + i * 12, 12);
                }

                int startOffset = (FileTable[LevelFileTableOffset[(int)SelectedLevel]].StartOffset & 0x7FFFFFFF) + 0x1B2C6C;
                
                byte[] referenceData = new byte[4];

                foreach (int offset in HardcodedReferences)
                {
                    Array.Copy(romArray, offset, referenceData, 0, 4);
                    if(BitConverter.IsLittleEndian)
                        Array.Reverse(referenceData);
                    uint reference = BitConverter.ToUInt32(referenceData, 0);
                    if (reference > startOffset)
                        reference += (uint)_dSize + (uint)padLength;
                    referenceData = BitConverter.GetBytes(reference);
                    if(BitConverter.IsLittleEndian)
                        Array.Reverse(referenceData);
                    Array.Copy(referenceData, 0, romArray, offset, 4);
                }

                //CRC fixing
                bool fixedd = N64Sums.FixChecksum(romArray);
                if(!fixedd)
                    fixedd = true;

                using (StreamWriter writer = new StreamWriter(saveFileDialog.FileName))
                {
                    writer.BaseStream.Write(romArray, 0, romArray.Length);
                }
            }
        }

        private void btnLoadLevel_Click(object sender, EventArgs e)
        {
            btnSaveLevel.Enabled = false;

            SelectedLevel = (LevelNames)cbLevel.SelectedIndex;

            //Locate the start of the data + the compressed length, and then decompress it
            int startOffset = (FileTable[LevelFileTableOffset[(int)SelectedLevel]].StartOffset & 0x7FFFFFFF) + 0x1B2C6C;
            int endOffset = startOffset + FileTable[LevelFileTableOffset[(int)SelectedLevel]].CompressedSize * 4;

            byte[] compressedLevelData = new byte[endOffset - startOffset];
            byte[] romArray = _romData.ToArray();

            Array.Copy(romArray, startOffset, compressedLevelData, 0, compressedLevelData.Length);

            //Decompress
            using (StreamWriter writer = new StreamWriter("tempCompressedVPK0Data.bin"))
            {
                writer.BaseStream.Write(compressedLevelData, 0, compressedLevelData.Length);
            }

            string nvpkCommand = @"/c nvpktool -d -level 2 -i tempCompressedVPK0Data.bin -o tempDecompressedVPK0Data.bin";

            //System.Diagnostics.Process.Start("CMD.exe", nvpkCommand);
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            //startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = nvpkCommand;
            process.StartInfo = startInfo;
            process.Start(); ///NOT WORKING!!!

            Thread.Sleep(500); //wait a bit

            _decompressedLevelData = new List<byte>();

            if (File.Exists("tempDecompressedVPK0Data.bin"))
            {
                _decompressedLevelData.AddRange(File.ReadAllBytes("tempDecompressedVPK0Data.bin"));

                File.Delete("tempDecompressedVPK0Data.bin");

                btnSaveLevel.Enabled = true;
            }

            File.Delete("tempCompressedVPK0Data.bin");

            //Possibly have a pre-made palette/texture offset
            switch(SelectedLevel)
            {
                case LevelNames.HyruleTemple:
                    LoadHyruleTextures(_decompressedLevelData);
                    break;
            }

            cbTextures.Enabled = true;
        }

        private int _dSize;

        private void btnSaveLevel_Click(object sender, EventArgs e)
        {
            //if(!Textures.Empty)
            //    SaveTextures();

            //Recompress back into the rom data
            byte[] decompressedArray = _decompressedLevelData.ToArray();

            //Add back in the texture data
            foreach (Palette palette in _palettes)
            {
                byte[] paletteData = palette.RawData;
                Array.Copy(paletteData, 0, decompressedArray, palette.FileOffset, paletteData.Length);
            }

            foreach (Texture texture in _textures)
            {
                byte[] textureData = texture.RawData;
                Array.Copy(textureData, 0, decompressedArray, texture.FileOffset, textureData.Length);
            }

            using (StreamWriter writer = new StreamWriter("tempDecompressedVPK0Data.bin"))
            {
                writer.BaseStream.Write(decompressedArray, 0, decompressedArray.Length);
            }

            string nvpkCommand = @"/c nvpktool -c -level 2 -i tempDecompressedVPK0Data.bin -o tempCompressedVPK0Data.bin";

            //System.Diagnostics.Process.Start("CMD.exe", nvpkCommand);
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            //startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = nvpkCommand;
            process.StartInfo = startInfo;
            process.Start(); ///NOT WORKING!!!

            Thread.Sleep(500); //wait a bit

            byte[] compressedLevelData = null;

            if (File.Exists("tempCompressedVPK0Data.bin"))
            {
                compressedLevelData = File.ReadAllBytes("tempCompressedVPK0Data.bin");

                File.Delete("tempCompressedVPK0Data.bin");

                btnSaveLevel.Enabled = true;
            }

            File.Delete("tempDecompressedVPK0Data.bin");

            //inject back into the rom data
            if (compressedLevelData == null)
                return;

            int paddedCompressedLength = (int)(Math.Ceiling((double)compressedLevelData.Length / 4) * 4);

            if (compressedLevelData.Length != paddedCompressedLength)
            {
                byte[] newCompressedData = new byte[paddedCompressedLength];
                Array.Copy(compressedLevelData, 0, newCompressedData, 0, compressedLevelData.Length);
                compressedLevelData = newCompressedData;
            }

            int startOffset = (FileTable[LevelFileTableOffset[(int)SelectedLevel]].StartOffset & 0x7FFFFFFF) + 0x1B2C6C;
            int oldSize = FileTable[LevelFileTableOffset[(int)SelectedLevel]].CompressedSize * 4;
            int sizeDiff = compressedLevelData.Length - oldSize;
            _dSize = sizeDiff;

            byte[] newRomData = new byte[_romData.Count + sizeDiff];

            //Remove the previous data
            for (int i = 0; i < startOffset; i++)
                newRomData[i] = _romData[i];

            for (int i = 0; i < compressedLevelData.Length; i++)
                newRomData[i + startOffset] = compressedLevelData[i];

            for (int i = startOffset + oldSize; i < _romData.Count; i++)
                newRomData[i + sizeDiff] = _romData[i];

            _romData = newRomData.ToList();

            //Now fix the file tables
            int fileOffset = LevelFileTableOffset[(int)SelectedLevel];

            FileTableEntry entry = FileTable[fileOffset];

            entry.DecompressedSize = (short)(_decompressedLevelData.Count / 4);
            entry.CompressedSize = (short)(compressedLevelData.Length / 4);

            FileTable[fileOffset] = entry;

            fileOffset++;

            for (; fileOffset < FileTable.Count; fileOffset++)
            {
                entry = FileTable[fileOffset];

                entry.StartOffset += sizeDiff;

                FileTable[fileOffset] = entry;
            }


            //Done!
        }


        #region Texture/Palette code

        List<Palette> _palettes = new List<Palette>();
        List<Texture> _textures = new List<Texture>();

        private int[] HyrulePaletteLocations = { 0x8, 0x838, 0xC68, 0xE98, 0x16C8, 0x1AF8, 0x1D28, 0x2558, 0x2788, 0x2838, 0x2A68, 0x2C98, 0x2EC8 };

        private int[] HyruleTextureLocations = { 0x30, 0x860, 0xC90, 0xEC0, 0x16F0, 0x1B20, 0x1D50, 0x2580, 0x27B0, 0x2860, 0x2A90, 0x2CC0, 0x2EF0 };

        private class TextureWrapper
        {
            public Texture Texture;
            public int TextureCount;
            
            public TextureWrapper(int i, Texture t)
            {
                TextureCount = i;
                Texture = t;
            }

            public override string ToString()
            {
                string formatString;
                string byteSizeString;

                //To do later
                formatString = "CI";
                byteSizeString = "4";

                return string.Format("Texture {0} ({1}x{2}, {3}{4}, palette size {5})",
                    TextureCount, Texture.Width, Texture.Height,
                    formatString, byteSizeString, Texture.ImagePalette.Colors.Length);

            }
        }

        private void LoadHyruleTextures(List<byte> romData)
        {
            _palettes.Clear();
            _textures.Clear();
            cbTextures.Items.Clear();

            Palette palette = null;
            Texture texture = null;
            byte[] paletteData = new byte[0x20];
            byte[] textureData6464 = new byte[64 * 64 / 2]; //All textures are CI4, so we can calc the size like this
            byte[] textureData6432 = new byte[64 * 32 / 2];
            byte[] textureData3232 = new byte[32 * 32 / 2];
            byte[] textureData1616 = new byte[16 * 16 / 2];

            for (int i = 0; i < HyrulePaletteLocations.Length; i++)
            {
                romData.CopyTo(HyrulePaletteLocations[i], paletteData, 0, 0x20);//All are 16 colors
                palette = new Palette(HyrulePaletteLocations[i], paletteData);
                switch (i)
                {
                    case 0:
                    case 3:
                    case 6:
                        //64x64
                        romData.CopyTo(HyruleTextureLocations[i], textureData6464, 0, textureData6464.Length);
                        texture = new Texture(HyruleTextureLocations[i], textureData6464, Texture.ImageFormat.CI, Texture.PixelInfo.Size_4b,
                            64, 64, palette, 0);
                        break;
                    case 1:
                    case 4:
                        //64x32
                        romData.CopyTo(HyruleTextureLocations[i], textureData6432, 0, textureData6432.Length);
                        texture = new Texture(HyruleTextureLocations[i], textureData6432, Texture.ImageFormat.CI, Texture.PixelInfo.Size_4b,
                            64, 32, palette, 0);
                        break;
                    case 2:
                    case 5:
                    case 7:
                    case 9:
                    case 10:
                    case 11:
                        //32x32
                        romData.CopyTo(HyruleTextureLocations[i], textureData3232, 0, textureData3232.Length);
                        texture = new Texture(HyruleTextureLocations[i], textureData3232, Texture.ImageFormat.CI, Texture.PixelInfo.Size_4b,
                            32, 32, palette, 0);
                        break;
                    case 8:
                        //16x16
                        romData.CopyTo(HyruleTextureLocations[i], textureData1616, 0, textureData1616.Length);
                        texture = new Texture(HyruleTextureLocations[i], textureData1616, Texture.ImageFormat.CI, Texture.PixelInfo.Size_4b,
                            16, 16, palette, 0);
                        break;
                    case 12:
                        //64x16
                        romData.CopyTo(HyruleTextureLocations[i], textureData3232, 0, textureData3232.Length);
                        texture = new Texture(HyruleTextureLocations[i], textureData3232, Texture.ImageFormat.CI, Texture.PixelInfo.Size_4b,
                            64, 16, palette, 0);
                        break;
                }

                _palettes.Add(palette);
                _textures.Add(texture);

            }

            foreach (Texture text in _textures)
            {
                if (text.IsValidFormat)
                {
                    cbTextures.Items.Add(new TextureWrapper(cbTextures.Items.Count + 1, text));
                }
            }

            if(cbTextures.Items.Count > 0)
                cbTextures.SelectedIndex = 0;
            /*Combos: (Palette/Texture, all in 16 color-CI4 format)
                        8, 30 (64x64)
                        838, 860 (64x32)
                        C68, C90 (32x32)
                        E98, EC0 (64x64)
                        16C8, 16F0 (64x32)
                        1AF8, 1B20 (32x32)
                        1D28, 1D50 (64x64) 
                        2558, 2580 (32x32)
                        2788, 27B0 (16x16)
                        2838, 2860 (32x32)
                        2A68, 2A90 (32x32)
                        2C98, 2CC0 (32x32)
                        2EC8, 2EF0 (64x16)
             */
        }

        #endregion

        private Texture SelectedTexture
        {
            get
            {
                if (cbTextures.SelectedItem != null && cbTextures.SelectedItem is TextureWrapper)
                {
                    return _textures[cbTextures.SelectedIndex];//((TextureWrapper)cbTextures.SelectedItem).Texture;
                }
                return null;
            }
        }

        private void cbTextures_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SelectedTexture != null)
            {
                pictureBox.Image = SelectedTexture.Image;

                btnExportImage.Enabled = true;
                btnImportImage.Enabled = true;
            }
            else
            {
                pictureBox.Image = null;
                btnExportImage.Enabled = false;
                btnImportImage.Enabled = false;
            }
        }

        private void btnExportImage_Click(object sender, EventArgs e)
        {
            if (saveImageDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                SelectedTexture.Image.Save(saveImageDialog.FileName);
            }
        }

        private void btnImportImage_Click(object sender, EventArgs e)
        {
            if (openImageDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Bitmap bmp = (Bitmap)Bitmap.FromFile(openImageDialog.FileName);

                pictureBox.Image = bmp;

                //convert to the correct format, or not
                
                byte[] rawData = TextureConversion.CI4ToBinary(bmp, SelectedTexture.ImagePalette, SelectedTexture.PaletteIndex, true);
                SelectedTexture.RawData = rawData;

                pictureBox.Image = SelectedTexture.Image;

            }
        }

    }
}
