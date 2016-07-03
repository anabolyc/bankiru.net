Imports System.Configuration
Imports System.Net
Imports www.BankBals.IPNumbers

Namespace Controllers

    Public Class FilterIPAttribute
        Inherits AuthorizeAttribute

#Region "Allowed"
        ''' Comma seperated string of allowable IPs. Example "10.2.5.41,192.168.0.22"
        Public Property AllowedSingleIPs() As String
            Get
                Return m_AllowedSingleIPs
            End Get
            Set(ByVal value As String)
                m_AllowedSingleIPs = value
            End Set
        End Property
        Private m_AllowedSingleIPs As String

        ''' Comma seperated string of allowable IPs with masks. Example "10.2.0.0;255.255.0.0,10.3.0.0;255.255.0.0"
        Public Property AllowedMaskedIPs() As String
            Get
                Return m_AllowedMaskedIPs
            End Get
            Set(ByVal value As String)
                m_AllowedMaskedIPs = value
            End Set
        End Property
        Private m_AllowedMaskedIPs As String

        ''' Gets or sets the configuration key for allowed single IPs
        Public Property ConfigurationKeyAllowedSingleIPs() As String
            Get
                Return m_ConfigurationKeyAllowedSingleIPs
            End Get
            Set(ByVal value As String)
                m_ConfigurationKeyAllowedSingleIPs = value
            End Set
        End Property
        Private m_ConfigurationKeyAllowedSingleIPs As String

        ''' Gets or sets the configuration key allowed mmasked IPs
        Public Property ConfigurationKeyAllowedMaskedIPs() As String
            Get
                Return m_ConfigurationKeyAllowedMaskedIPs
            End Get
            Set(ByVal value As String)
                m_ConfigurationKeyAllowedMaskedIPs = value
            End Set
        End Property
        Private m_ConfigurationKeyAllowedMaskedIPs As String

        Private allowedIPListToCheck As New IPList()
#End Region

#Region "Denied"
        ''' Comma seperated string of denied IPs. Example "10.2.5.41,192.168.0.22"
        Public Property DeniedSingleIPs() As String
            Get
                Return m_DeniedSingleIPs
            End Get
            Set(ByVal value As String)
                m_DeniedSingleIPs = value
            End Set
        End Property
        Private m_DeniedSingleIPs As String

        ''' Comma seperated string of denied IPs with masks. Example "10.2.0.0;255.255.0.0,10.3.0.0;255.255.0.0"
        Public Property DeniedMaskedIPs() As String
            Get
                Return m_DeniedMaskedIPs
            End Get
            Set(ByVal value As String)
                m_DeniedMaskedIPs = value
            End Set
        End Property
        Private m_DeniedMaskedIPs As String

        ''' Gets or sets the configuration key for denied single IPs
        Public Property ConfigurationKeyDeniedSingleIPs() As String
            Get
                Return m_ConfigurationKeyDeniedSingleIPs
            End Get
            Set(ByVal value As String)
                m_ConfigurationKeyDeniedSingleIPs = value
            End Set
        End Property
        Private m_ConfigurationKeyDeniedSingleIPs As String

        ''' Gets or sets the configuration key for denied masked IPs
        Public Property ConfigurationKeyDeniedMaskedIPs() As String
            Get
                Return m_ConfigurationKeyDeniedMaskedIPs
            End Get
            Set(ByVal value As String)
                m_ConfigurationKeyDeniedMaskedIPs = value
            End Set
        End Property
        Private m_ConfigurationKeyDeniedMaskedIPs As String

        Private deniedIPListToCheck As New IPList()

#End Region

        ''' Determines whether access to the core framework is authorized.
        ''' <param name="httpContext">The HTTP context, which encapsulates all HTTP-specific information about an individual HTTP request.</param>
        Protected Overrides Function AuthorizeCore(ByVal httpContext As HttpContextBase) As Boolean
            If httpContext Is Nothing Then
                Throw New ArgumentNullException("httpContext")
            End If

            Dim userIpAddress As String = httpContext.Request.UserHostAddress

            Try
                ' Check that the IP is allowed to access
                Dim ipAllowed As Boolean = CheckAllowedIPs(userIpAddress)

                ' Check that the IP is not denied to access
                Dim ipDenied As Boolean = CheckDeniedIPs(userIpAddress)
                ' Only allowed if allowed and not denied
                Dim finallyAllowed As Boolean = ipAllowed AndAlso Not ipDenied

                Return finallyAllowed
                ' Log the exception, probably something wrong with the configuration
            Catch e As Exception
            End Try

            Return True
            ' if there was an exception, then we return true
        End Function

        ''' Checks the allowed IPs.
        Private Function CheckAllowedIPs(ByVal userIpAddress As String) As Boolean
            ' Populate the IPList with the Single IPs
            If Not String.IsNullOrEmpty(AllowedSingleIPs) Then
                SplitAndAddSingleIPs(AllowedSingleIPs, allowedIPListToCheck)
            End If

            ' Populate the IPList with the Masked IPs
            If Not String.IsNullOrEmpty(AllowedMaskedIPs) Then
                SplitAndAddMaskedIPs(AllowedMaskedIPs, allowedIPListToCheck)
            End If

            ' Check if there are more settings from the configuration (Web.config)
            If Not String.IsNullOrEmpty(ConfigurationKeyAllowedSingleIPs) Then
                Dim configurationAllowedAdminSingleIPs As String = ConfigurationManager.AppSettings(ConfigurationKeyAllowedSingleIPs)
                If Not String.IsNullOrEmpty(configurationAllowedAdminSingleIPs) Then
                    SplitAndAddSingleIPs(configurationAllowedAdminSingleIPs, allowedIPListToCheck)
                End If
            End If

            If Not String.IsNullOrEmpty(ConfigurationKeyAllowedMaskedIPs) Then
                Dim configurationAllowedAdminMaskedIPs As String = ConfigurationManager.AppSettings(ConfigurationKeyAllowedMaskedIPs)
                If Not String.IsNullOrEmpty(configurationAllowedAdminMaskedIPs) Then
                    SplitAndAddMaskedIPs(configurationAllowedAdminMaskedIPs, allowedIPListToCheck)
                End If
            End If

            Return allowedIPListToCheck.CheckNumber(userIpAddress)
        End Function

        ''' Checks the denied IPs.
        Private Function CheckDeniedIPs(ByVal userIpAddress As String) As Boolean
            ' Populate the IPList with the Single IPs
            If Not String.IsNullOrEmpty(DeniedSingleIPs) Then
                SplitAndAddSingleIPs(DeniedSingleIPs, deniedIPListToCheck)
            End If

            ' Populate the IPList with the Masked IPs
            If Not String.IsNullOrEmpty(DeniedMaskedIPs) Then
                SplitAndAddMaskedIPs(DeniedMaskedIPs, deniedIPListToCheck)
            End If

            ' Check if there are more settings from the configuration (Web.config)
            If Not String.IsNullOrEmpty(ConfigurationKeyDeniedSingleIPs) Then
                Dim configurationDeniedAdminSingleIPs As String = ConfigurationManager.AppSettings(ConfigurationKeyDeniedSingleIPs)
                If Not String.IsNullOrEmpty(configurationDeniedAdminSingleIPs) Then
                    SplitAndAddSingleIPs(configurationDeniedAdminSingleIPs, deniedIPListToCheck)
                End If
            End If

            If Not String.IsNullOrEmpty(ConfigurationKeyDeniedMaskedIPs) Then
                Dim configurationDeniedAdminMaskedIPs As String = ConfigurationManager.AppSettings(ConfigurationKeyDeniedMaskedIPs)
                If Not String.IsNullOrEmpty(configurationDeniedAdminMaskedIPs) Then
                    SplitAndAddMaskedIPs(configurationDeniedAdminMaskedIPs, deniedIPListToCheck)
                End If
            End If

            Return deniedIPListToCheck.CheckNumber(userIpAddress)
        End Function

        ''' Splits the incoming ip string of the format "IP,IP" example "10.2.0.0,10.3.0.0" and adds the result to the IPList
        Private Sub SplitAndAddSingleIPs(ByVal ips As String, ByVal list As IPList)
            Dim splitSingleIPs = ips.Split(","c)
            For Each ip As String In splitSingleIPs
                list.Add(ip)
            Next
        End Sub

        ''' Splits the incoming ip string of the format "IP;MASK,IP;MASK" example "10.2.0.0;255.255.0.0,10.3.0.0;255.255.0.0" and adds the result to the IPList
        Private Sub SplitAndAddMaskedIPs(ByVal ips As String, ByVal list As IPList)
            Dim splitMaskedIPs = ips.Split(","c)
            For Each maskedIp As String In splitMaskedIPs
                Dim ipAndMask = maskedIp.Split(";"c)
                ' IP;MASK
                list.Add(ipAndMask(0), ipAndMask(1))
            Next
        End Sub

        Public Overrides Sub OnAuthorization(ByVal filterContext As AuthorizationContext)
            MyBase.OnAuthorization(filterContext)
        End Sub

    End Class

    Public Class InternalOnly
        Inherits FilterAttribute

        Public Sub OnAuthorization(ByVal filterContext As AuthorizationContext)
            If Not IsIntranet(filterContext.HttpContext.Request.UserHostAddress) Then
                Throw New HttpException(CInt(HttpStatusCode.Forbidden), "Access forbidden.")
            End If
        End Sub

        Private Function IsIntranet(ByVal userIP As String) As Boolean
            ' match an internal IP (ex: 127.0.0.1)
            Return Not String.IsNullOrEmpty(userIP) AndAlso Regex.IsMatch(userIP, "^127")
        End Function
    End Class

End Namespace
