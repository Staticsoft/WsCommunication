param(
    [string] $Stage = 'Dev'
)

$StackName = "WebSocketCommunicationServices$Stage"
$ParametersFileName = "Parameters$Stage.json"

function Deploy-Stack {
    $stack = Find-Stack
    if ($null -eq $stack) {
        New-Stack
    }
    else {
        Update-Stack
    }
}

function Find-Stack {
    & aws @(
        'cloudformation'
        'describe-stacks'
        '--stack-name'
        $StackName
    ) 2>$null
}

function New-Stack {
    Write-Stack -Command 'create-stack'
}

function Update-Stack {
    Write-Stack -Command 'update-stack'
}

function Write-Stack {
    param(
        [string] $command
    )

    & aws @(
        'cloudformation'
        $command
        '--stack-name'
        $StackName
        '--template-body'
        "file://$PSScriptRoot/templates/Services.yml"
        '--parameters'
        "file://$PSScriptRoot/../.local/deploy/$ParametersFileName"
        '--capabilities'
        'CAPABILITY_NAMED_IAM'
        "--tags=Key=product,Value=WebSocketCommunication${Stage}"
    )
}

Deploy-Stack