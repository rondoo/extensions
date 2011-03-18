﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Signum.Entities;

namespace Signum.Entities.Mailing
{
    public enum SMTPConfigurationQueries
    { 
        NoCredentialsData
    }

    [Serializable]
    public class SMTPConfigurationDN : Entity
    {
        [NotNullable, SqlDbType(Size = 100), UniqueIndex]
        string name;
        [StringLengthValidator(AllowNulls = false, Min = 1, Max = 100)]
        public string Name
        {
            get { return name; }
            set { SetToStr(ref name, value, () => Name); }
        }

        int port = 25;
        public int Port
        {
            get { return port; }
            set { Set(ref port, value, () => Port); }
        }

        string host;
        public string Host
        {
            get { return host; }
            set { Set(ref host, value, () => Host); }
        }

        bool useDefaultCredentials = true;
        public bool UseDefaultCredentials
        {
            get { return useDefaultCredentials; }
            set { Set(ref useDefaultCredentials, value, () => UseDefaultCredentials); }
        }

        string username;
        public string Username
        {
            get { return username; }
            set { Set(ref username, value, () => Username); }
        }

        string password;
        public string Password
        {
            get { return password; }
            set { Set(ref password, value, () => Password); }
        }

        bool enableSSL;
        public bool EnableSSL
        {
            get { return enableSSL; }
            set { Set(ref enableSSL, value, () => EnableSSL); }
        }

        MList<ClientCertificationFileDN> clientCertificationFiles;
        public MList<ClientCertificationFileDN> ClientCertificationFiles
        {
            get { return clientCertificationFiles; }
            set { Set(ref clientCertificationFiles, value, () => ClientCertificationFiles); }
        }


        public override string ToString()
        {
            return name;
        }
    }

    [Serializable]
    public class ClientCertificationFileDN : Entity
    {
        [NotNullable, SqlDbType(Size = 100), UniqueIndex]
        string name;
        [StringLengthValidator(AllowNulls = false, Min = 1, Max = 100)]
        public string Name
        {
            get { return name; }
            set { SetToStr(ref name, value, () => Name); }
        }

        string fullFilePath;
        public string FullFilePath
        {
            get { return fullFilePath; }
            set { Set(ref fullFilePath, value, () => FullFilePath); }
        }

        CertFileType certFileType;
        public CertFileType CertFileType
        {
            get { return certFileType; }
            set { Set(ref certFileType, value, () => CertFileType); }
        }

        public override string ToString()
        {
            return name;
        }
    }

    public enum CertFileType
    { 
        CertFile,
        SignedFile
    }
}