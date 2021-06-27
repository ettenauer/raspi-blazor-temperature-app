# raspi-blazor-temperature-app
The project is part of proof of concept system to verify following technologies:
* Edge to Cloud/K8s (with Raspi)
* Services hosted in K8s using Helm
* Services hosted in AKS using Helm
* Blazor WebAssembly
* Azure Identity 
* Inner Dev Loop with Docker-Compose and MiniKube

![System Design](https://github.com/ettenauer/raspi-blazor-temperature-app/blob/master/images/SystemDesign.PNG)

# Prerequisites
* Azure account to configure Azure Identity (Scope and AppRole)
* K8s cluster setup

# Inner Dev Loop with Docker-Compose
* open powershell/cmd
* navigate to docker-compose folder in repo
* docker-compose up

# Run on Docker Desktop K8s with Helm
## Cluster Setup
* install Docker Destop and activate K8s
* create secret for image repository:
** kubectl create secret docker-registry private-docker-registry-cred --docker-server=https://ghcr.io --docker-username=<USERNAME> --docker-password=<PASSWORD> --namespace=default -o yaml
* create secret self-signed TLS certificate for ingress (CN -> *.local):
** openssl req -x509 -newkey rsa:4096 -sha256 -nodes -keyout tls_self.key -out tls_self.crt -days 365 -subj "/CN=*.local" -days 365
** kubectl create secret tls local-tls --cert=tls_self.crt --key=tls_self.key+
** install helm

## Run Application
* helm install local ./helm 
* app should accessible via ingress -> https://raspi.local/
* helm uninstall local 
