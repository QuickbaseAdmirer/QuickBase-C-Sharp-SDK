/*
 * Copyright © 2010 Intuit Inc. All rights reserved.
 * All rights reserved. This program and the accompanying materials
 * are made available under the terms of the Eclipse Public License v1.0
 * which accompanies this distribution, and is available at
 * http://www.opensource.org/licenses/eclipse-1.0.php
 */
using System.Net;
using System.IO;
using System.Xml.XPath;
using Intuit.QuickBase.Core.Exceptions;

namespace Intuit.QuickBase.Core
{
    internal class Http
    {
        internal XPathDocument Get(IQGetObject apiAction)
        {
            WebResponse response = null;
            Stream responseStream = null;
            XPathDocument xml;

            try
            {
                var request = (HttpWebRequest)WebRequest.Create(apiAction.Uri);
                request.Method = "GET";
                request.ProtocolVersion = HttpVersion.Version10;
                request.KeepAlive = false;
                request.Timeout = 300000;

                response = request.GetResponse();
                responseStream = response.GetResponseStream();
                xml = new XPathDocument(responseStream);
            }
            finally
            {
                if (responseStream != null) responseStream.Close();
                if (response != null) response.Close();
            }

            CheckForException(xml);
            return xml;
        }

        internal void GetFile(DownloadFile downloadFile)
        {
            BinaryReader br = null;
            BinaryWriter bw = null;
            WebResponse response = null;
            Stream responseStream = null;
            
            try
            {
                // Request
                var request = (HttpWebRequest)WebRequest.Create(downloadFile.Uri);
                request.Method = "GET";
                request.AllowWriteStreamBuffering = false;
                request.KeepAlive = false;
                request.Timeout = 300000;
                var cookie = new Cookie("TICKET", downloadFile.Ticket);
                request.CookieContainer = new CookieContainer();
                request.CookieContainer.Add(new System.Uri(downloadFile.Uri), cookie);

                // Response
                response = request.GetResponse();
                responseStream = response.GetResponseStream();

                // Write file
                if (!Directory.Exists(downloadFile.Path))
                {
                    Directory.CreateDirectory(downloadFile.Path);
                }
                br = new BinaryReader(responseStream);
                bw = new BinaryWriter(File.Create(downloadFile.Path + @"\" + downloadFile.File));
                bw.Write(br.ReadBytes((int)response.ContentLength));
                bw.Flush();
            }
            finally
            {
                if (bw != null) bw.Close();
                if (br != null) br.Close();
                if (responseStream != null) responseStream.Close();
                if (response != null) response.Close();
            }
        }

        internal static void CheckForException(XPathDocument xml)
        {
            var xmlNav = xml.CreateNavigator();
            string errorcode = xmlNav.SelectSingleNode("/qdbapi/errcode").Value;
            string errortext = xmlNav.SelectSingleNode("/qdbapi/errtext").Value;
            var errDetailNode = xmlNav.SelectSingleNode("/qdbapi/errdetail");
            if (errDetailNode != null)
            {
                string errdetail = errDetailNode.Value;
                if (errdetail.Length > 0) errortext += ":" + errdetail;
            }
            if ("2".Equals(errorcode))
            {
                throw new InvalidInputException(errortext);
            }

            if ("3".Equals(errorcode))
            {
                throw new InsufficientPermissionsException(errortext);
            }

            if ("4".Equals(errorcode))
            {
                throw new BadTicketException(errortext);
            }

            if ("5".Equals(errorcode))
            {
                throw new UnimplementedOperationException(errortext);
            }

            if ("6".Equals(errorcode))
            {
                throw new SyntaxErrorException(errortext);
            }

            if ("7".Equals(errorcode))
            {
                throw new ApiNotAllowedOnApplicationTableException(errortext);
            }

            if ("8".Equals(errorcode))
            {
                throw new SslRequiredOnTableException(errortext);
            }

            if ("9".Equals(errorcode))
            {
                throw new InvalidChoiceException(errortext);
            }

            if ("11".Equals(errorcode))
            {
                throw new UnableToParseXmlException(errortext);
            }

            if ("12".Equals(errorcode))
            {
                throw new InvalidSourceDbidException(errortext);
            }

            if ("13".Equals(errorcode))
            {
                throw new InvalidAccountIDException(errortext);
            }

            if ("14".Equals(errorcode))
            {
                throw new MissingDbidOrWrongTypeException(errortext);
            }

            if ("20".Equals(errorcode))
            {
                throw new UnknownUsernamePasswordException(errortext);
            }

            if ("21".Equals(errorcode))
            {
                throw new UnknownUserException(errortext);
            }

            if ("22".Equals(errorcode))
            {
                throw new SignInRequiredException(errortext);
            }

            if ("23".Equals(errorcode))
            {
                throw new FeatureNotSupportedException(errortext);
            }

            if ("24".Equals(errorcode))
            {
                throw new InvalidApplicationTokenException(errortext);
            }

            if ("30".Equals(errorcode))
            {
                throw new NoSuchRecordException(errortext);
            }

            if ("31".Equals(errorcode))
            {
                throw new NoSuchFieldException(errortext);
            }

            if ("32".Equals(errorcode))
            {
                throw new ApplicationDoesNotExistOrDeletedException(errortext);
            }

            if ("33".Equals(errorcode))
            {
                throw new NoSuchQueryException(errortext);
            }

            if ("34".Equals(errorcode))
            {
                throw new CannotChangeValueOfFieldException(errortext);
            }

            if ("35".Equals(errorcode))
            {
                throw new NoDataReturnedException(errortext);
            }

            if ("36".Equals(errorcode))
            {
                throw new CloningException(errortext);
            }

            if ("37".Equals(errorcode))
            {
                throw new NoSuchReportException(errortext);
            }

            if ("38".Equals(errorcode))
            {
                throw new PeriodicReportContainsRestrictedFieldException(errortext);
            }

            if ("50".Equals(errorcode))
            {
                throw new MissingRequiredFieldException(errortext);
            }

            if ("51".Equals(errorcode))
            {
                throw new AttemptingToAddNonUniqueValueException(errortext);
            }

            if ("53".Equals(errorcode))
            {
                throw new RequiredFieldsMissingFromImportException(errortext);
            }

            if ("54".Equals(errorcode))
            {
                throw new CachedRecordsNotFoundException(errortext);
            }

            if ("60".Equals(errorcode))
            {
                throw new UpdateConflictDetectedException(errortext);
            }

            if ("70".Equals(errorcode))
            {
                throw new AccountSizeLimitExceededException(errortext);
            }

            if ("71".Equals(errorcode))
            {
                throw new DatabaseSizeLimitExceededException(errortext);
            }

            if ("73".Equals(errorcode))
            {
                throw new AccountSuspendedException(errortext);
            }

            if ("74".Equals(errorcode))
            {
                throw new NotAllowedToCreateApplicationException(errortext);
            }

            if ("75".Equals(errorcode))
            {
                throw new ViewTooLargeException(errortext);
            }

            if ("76".Equals(errorcode))
            {
                throw new TooManyCriteriaInQueryException(errortext);
            }

            if ("77".Equals(errorcode))
            {
                throw new ApiRequestLimitExceededException(errortext);
            }

            if ("80".Equals(errorcode))
            {
                throw new OverflowException(errortext);
            }

            if ("81".Equals(errorcode))
            {
                throw new ItemNotFoundException(errortext);
            }

            if ("82".Equals(errorcode))
            {
                throw new OperationTookTooLongException(errortext);
            }

            if ("100".Equals(errorcode))
            {
                throw new TechnicalDifficultiesException(errortext);
            }

            if ("101".Equals(errorcode))
            {
                throw new NoSuchDatabaseException(errortext);
            }

            if ("110".Equals(errorcode))
            {
                throw new InvalidRoleException(errortext);
            }

            if ("111".Equals(errorcode))
            {
                throw new UserExistsException(errortext);
            }

            if ("112".Equals(errorcode))
            {
                throw new NoUserInRoleException(errortext);
            }

            if ("113".Equals(errorcode))
            {
                throw new UserAlreadyInRoleException(errortext);
            }
        }
    }
}