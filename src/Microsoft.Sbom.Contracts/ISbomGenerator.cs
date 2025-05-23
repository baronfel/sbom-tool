// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.Sbom.Contracts.Enums;

namespace Microsoft.Sbom.Contracts;

/// <summary>
/// Provides an API interface to the SBOM generator workflow.
/// </summary>
public interface ISbomGenerator
{
    /// <summary>
    /// Generate a SBOM in the rootPath using the provided file and package lists.
    /// </summary>
    /// <param name="rootPath">The root path of the drop where the generated SBOM will be placed.</param>
    /// <param name="files">The list of <see cref="SbomFile">files</see> to include in this SBOM.</param>
    /// <param name="packages">The list of <see cref="SbomPackage">packages</see> to include in this SBOM.</param>
    /// <param name="metadata">Provide any available metadata about your build environment using the SbomMetadata object.</param>
    /// <param name="specifications">Provide a list of <see cref="SbomSpecification"/> that you want your SBOM to be generated
    /// for. If this is not provided, we will generate SBOMs for all the available formats.</param>
    /// <param name="runtimeConfiguration">Configuration to tweak the SBOM generator workflow.</param>
    /// <param name="manifestDirPath">Output directory. If null defaults to rootPath.</param>
    /// <returns>The result object that indicates if the generation succeeded, and a list of
    /// errors if it failed along with telemetry.</returns>
    public Task<SbomGenerationResult> GenerateSbomAsync(
        string rootPath,
        IEnumerable<SbomFile> files,
        IEnumerable<SbomPackage> packages,
        SbomMetadata metadata,
        IList<SbomSpecification> specifications = null,
        RuntimeConfiguration runtimeConfiguration = null,
        string manifestDirPath = null,
        string externalDocumentReferenceListFile = null);

    /// <summary>
    /// Generate a SBOM by traversing the rootPath folder and discovering files and packages.
    /// </summary>
    /// <param name="rootPath">The root path of the drop where the generated SBOM will be placed.</param>
    /// <param name="componentPath">The path where all the build components for this drop can be found.</param>
    /// <param name="metadata">Provide any available metadata about your build environment using the SbomMetadata object.</param>
    /// <param name="specifications">Provide a list of <see cref="SbomSpecification"/> that you want your SBOM to be generated
    /// for. If this is not provided, we will generate SBOMs for all the available formats.</param>
    /// <param name="runtimeConfiguration">Configuration to tweak the SBOM generator workflow.</param>
    /// <param name="manifestDirPath">Output directory. If null defaults to rootPath joined to _manifest.</param>
    /// <returns>The result object that indicates if the generation succeeded, and a list of
    /// errors if it failed along with telemetry.</returns>
    public Task<SbomGenerationResult> GenerateSbomAsync(
        string rootPath,
        string componentPath,
        SbomMetadata metadata,
        IList<SbomSpecification> specifications = null,
        RuntimeConfiguration runtimeConfiguration = null,
        string manifestDirPath = null,
        string externalDocumentReferenceListFile = null);

    /// <summary>
    /// Each SBOM specification requires that each file and package have a specific list of hashes
    /// generated for them. Use this function to get a list of the required hash algorithms for your
    /// SBOM specification. The SBOM generator may throw an exception if a hash algorithm value is missing.
    /// </summary>
    /// <param name="specification">The SBOM specification.</param>
    /// <returns>A list of <see cref="HashAlgorithmName"/>.</returns>
    public IEnumerable<AlgorithmName> GetRequiredAlgorithms(SbomSpecification specification);

    /// <summary>
    /// Gets a list of <see cref="SbomSpecification"/> this SBOM generator supports.
    /// </summary>
    /// <returns>A list of <see cref="SbomSpecification"/>.</returns>
    public IEnumerable<SbomSpecification> GetSupportedSbomSpecifications();
}
