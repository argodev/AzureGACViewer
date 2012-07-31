<?xml version="1.0" encoding="utf-8"?>
<serviceModel xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" name="AzureGACViewer" generation="1" functional="0" release="0" Id="64ee680b-9253-49ec-9c02-7fee9de0428d" dslVersion="1.2.0.0" xmlns="http://schemas.microsoft.com/dsltools/RDSM">
  <groups>
    <group name="AzureGACViewerGroup" generation="1" functional="0" release="0">
      <componentports>
        <inPort name="GACViewerRole:HttpIn" protocol="http">
          <inToChannel>
            <lBChannelMoniker name="/AzureGACViewer/AzureGACViewerGroup/LB:GACViewerRole:HttpIn" />
          </inToChannel>
        </inPort>
      </componentports>
      <settings>
        <aCS name="GACViewerRole:DiagnosticsConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/AzureGACViewer/AzureGACViewerGroup/MapGACViewerRole:DiagnosticsConnectionString" />
          </maps>
        </aCS>
        <aCS name="GACViewerRoleInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/AzureGACViewer/AzureGACViewerGroup/MapGACViewerRoleInstances" />
          </maps>
        </aCS>
      </settings>
      <channels>
        <lBChannel name="LB:GACViewerRole:HttpIn">
          <toPorts>
            <inPortMoniker name="/AzureGACViewer/AzureGACViewerGroup/GACViewerRole/HttpIn" />
          </toPorts>
        </lBChannel>
      </channels>
      <maps>
        <map name="MapGACViewerRole:DiagnosticsConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/AzureGACViewer/AzureGACViewerGroup/GACViewerRole/DiagnosticsConnectionString" />
          </setting>
        </map>
        <map name="MapGACViewerRoleInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/AzureGACViewer/AzureGACViewerGroup/GACViewerRoleInstances" />
          </setting>
        </map>
      </maps>
      <components>
        <groupHascomponents>
          <role name="GACViewerRole" generation="1" functional="0" release="0" software="C:\workspaces\AzureGACViewer\AzureGACViewer\csx\Release\roles\GACViewerRole" entryPoint="base\x64\WaHostBootstrapper.exe" parameters="base\x64\WaIISHost.exe " memIndex="1792" hostingEnvironment="frontendadmin" hostingEnvironmentVersion="2">
            <componentports>
              <inPort name="HttpIn" protocol="http" portRanges="80" />
            </componentports>
            <settings>
              <aCS name="DiagnosticsConnectionString" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;GACViewerRole&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;GACViewerRole&quot;&gt;&lt;e name=&quot;HttpIn&quot; /&gt;&lt;/r&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/AzureGACViewer/AzureGACViewerGroup/GACViewerRoleInstances" />
            <sCSPolicyFaultDomainMoniker name="/AzureGACViewer/AzureGACViewerGroup/GACViewerRoleFaultDomains" />
          </sCSPolicy>
        </groupHascomponents>
      </components>
      <sCSPolicy>
        <sCSPolicyFaultDomain name="GACViewerRoleFaultDomains" defaultPolicy="[2,2,2]" />
        <sCSPolicyID name="GACViewerRoleInstances" defaultPolicy="[1,1,1]" />
      </sCSPolicy>
    </group>
  </groups>
  <implements>
    <implementation Id="7fb89e9b-886e-47cb-bb8d-d5b0b19a8dd8" ref="Microsoft.RedDog.Contract\ServiceContract\AzureGACViewerContract@ServiceDefinition.build">
      <interfacereferences>
        <interfaceReference Id="771d2e8a-ba22-4c5f-9eed-db08b9036bbb" ref="Microsoft.RedDog.Contract\Interface\GACViewerRole:HttpIn@ServiceDefinition.build">
          <inPort>
            <inPortMoniker name="/AzureGACViewer/AzureGACViewerGroup/GACViewerRole:HttpIn" />
          </inPort>
        </interfaceReference>
      </interfacereferences>
    </implementation>
  </implements>
</serviceModel>