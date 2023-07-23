package main

import (
	"context"
	"fmt"
	"os"

	"dagger.io/dagger"
)

func main() {
	client, err := dagger.Connect(context.Background(), dagger.WithLogOutput(os.Stdout))
	if err != nil {
		fmt.Println(err)
	}
	defer client.Close()

	src := client.Host().Directory("./src")
	tests := client.Host().Directory("./tests")

	container := client.Container().
		From("mcr.microsoft.com/dotnet/sdk:7.0").
		WithMountedDirectory("/app/src", src).
		WithMountedDirectory("app/tests", tests).
		WithWorkdir("/app/tests/CitiesApp.IntegrationTests").
		// WithEnvVariable("DOCKER_HOST", "tcp://host.docker.internal:2375").
		WithEnvVariable("DOCKER_HOST", "tcp://docker:2375").
		WithExec([]string{"dotnet", "test"})

	_, err = container.Stdout(context.Background())
	if err != nil {
		fmt.Println(err)
	}
}
