FROM gitpod/workspace-dotnet:latest

RUN /bin/sh -c curl -fsSL https://dot.net/v1/dotnet-install.sh | bash /dev/stdin --channel 5.0 --install-dir /home/gitpod/dotnet