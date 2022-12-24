# CleanMicroserviceSystem.Uranus.WebAPI

> Consul: https://developer.hashicorp.com/consul
>
> Download: https://developer.hashicorp.com/consul/downloads
>
> Resource: https://releases.hashicorp.com/consul
>
> Management UI: http://localhost:8500

# Launch Consul

```
consul agent -dev
```

# Register Service to Consul

Add `AgentServiceRegistrationConfiguration` configuration in Service's JSON configuration file.

```json
"AgentServiceRegistrationConfiguration": {
	"ServicesProviderAddress": "http://localhost:8500",
	"ServiceName": "IdentityServer-Themis",
	"ServiceInstanceId": "IdentityServer_Themis_000",
	"SelfHost": "localhost",
	"SelfPort": "11002",
	"HealthCheckUrl": "https://localhost:11002/health/healthcheck"
}
```

`services.AddGatewayServiceRegister(agentServiceRegistrationConfiguration)` to enable Gateway registration function.

You should see your services available in Consul Management UI after Service launched.

# Configure Gateway-Uranus to forward Service's request

Add new item in `Routes` collection of JSON configuration file of Gateway-Uranus Web API project.

```json
{
	"DownstreamPathTemplate": "/api/{url}",
	"DownstreamScheme": "https",
	"UpstreamPathTemplate": "/Ocelot/Themis/{url}",
	"UpstreamHttpMethod": [ "Get", "Post", "Put", "Patch", "Delete", "Options", "Head" ],
	"ServiceName": "IdentityServer-Themis",
	"UseServiceDiscovery": true,
	"LoadBalancerOptions": {
		"Type": "RoundRobin"
	}
}
```

# Access

Send HTTP[S] requests to Gateway-Uranus Web API project, requests will be forwarded to Service by Ocelot middleware.

```http
https://{Gateway-Uranus address}/Ocelot/{ServiceName}/{url}
Above requests will be forwarded to below address
https://{Service address}/api/{url}
```

 
