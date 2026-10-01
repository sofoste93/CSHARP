# Windows publisher identity

The release workflow supports Authenticode signing for the Windows executable. Add a publicly trusted PFX code-signing certificate as the `WINDOWS_CERTIFICATE_BASE64` secret and its password as `WINDOWS_CERTIFICATE_PASSWORD`.

When both secrets exist, GitHub Actions signs `VioletPulsar.exe` with SHA-256 and a trusted timestamp before creating the release archive. The certificate and password must never be committed to the repository.
