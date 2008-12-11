﻿#region License Information
/* HeuristicLab
 * Copyright (C) 2002-2008 Heuristic and Evolutionary Algorithms Laboratory (HEAL)
 *
 * This file is part of HeuristicLab.
 *
 * HeuristicLab is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * HeuristicLab is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with HeuristicLab. If not, see <http://www.gnu.org/licenses/>.
 */
#endregion

using HeuristicLab.Hive.Server.Core.InternalInterfaces.DataAccess;
using HeuristicLab.PluginInfrastructure;
using System.Runtime.CompilerServices;
using HeuristicLab.Hive.Contracts.Interfaces;

/// <summary>
/// The service locator for the server core
/// </summary>
public class ServiceLocator {
  private static DiscoveryService discoveryService =
    new DiscoveryService();

  private static ITransactionManager transManager = null;

  private static IClientAdapter clientAdapter = null;

  private static IClientGroupAdapter clientGroupAdapter = null;

  private static IResourceAdapter resourceAdapter = null;

  private static IUserAdapter userAdapter = null;

  private static IUserGroupAdapter userGroupAdapter = null;

  private static IPermissionOwnerAdapter permOwnerAdapter = null;

  private static IJobAdapter jobAdapter = null;

  /// <summary>
  /// Gets the db transaction manager
  /// </summary>
  /// <returns></returns>
  [MethodImpl(MethodImplOptions.Synchronized)]
  public static ITransactionManager GetTransactionManager() {
    if (transManager == null) {
      transManager = discoveryService.GetInstances<ITransactionManager>()[0];
    }

    return transManager;
  }

  /// <summary>
  /// Gets the client database adapter
  /// </summary>
  /// <returns></returns>
  [MethodImpl(MethodImplOptions.Synchronized)]
  public static IClientAdapter GetClientAdapter() {
    if (clientAdapter == null) {
      clientAdapter = discoveryService.GetInstances<IClientAdapter>()[0];
    }

    return clientAdapter;
  }

  /// <summary>
  /// Gets the client group database adapter
  /// </summary>
  /// <returns></returns>
  [MethodImpl(MethodImplOptions.Synchronized)]
  public static IClientGroupAdapter GetClientGroupAdapter() {
    if (clientGroupAdapter == null) {
      clientGroupAdapter = discoveryService.GetInstances<IClientGroupAdapter>()[0];
    }

    return clientGroupAdapter;
  }

  /// <summary>
  /// Gets the resource database adapter
  /// </summary>
  /// <returns></returns>
  [MethodImpl(MethodImplOptions.Synchronized)]
  public static IResourceAdapter GetResourceAdapter() {
    if (resourceAdapter == null) {
      resourceAdapter = discoveryService.GetInstances<IResourceAdapter>()[0];
    }

    return resourceAdapter;
  }

  /// <summary>
  /// Gets the user database adapter
  /// </summary>
  /// <returns></returns>
  [MethodImpl(MethodImplOptions.Synchronized)]
  public static IUserAdapter GetUserAdapter() {
    if (userAdapter == null) {
      userAdapter = discoveryService.GetInstances<IUserAdapter>()[0];
    }

    return userAdapter;
  }

  /// <summary>
  /// Gets the user group database adapter
  /// </summary>
  /// <returns></returns>
  [MethodImpl(MethodImplOptions.Synchronized)]
  public static IUserGroupAdapter GetUserGroupAdapter() {
    if (userGroupAdapter == null) {
      userGroupAdapter = discoveryService.GetInstances<IUserGroupAdapter>()[0];
    }

    return userGroupAdapter;
  }

  /// <summary>
  /// Gets the permission owner database adapter
  /// </summary>
  /// <returns></returns>
  [MethodImpl(MethodImplOptions.Synchronized)]
  public static IPermissionOwnerAdapter GetPermissionOwnerAdapter() {
    if (permOwnerAdapter == null) {
      permOwnerAdapter = discoveryService.GetInstances<IPermissionOwnerAdapter>()[0];
    }

    return permOwnerAdapter;
  }

  /// <summary>
  /// Gets the job database adapter
  /// </summary>
  /// <returns></returns>
  [MethodImpl(MethodImplOptions.Synchronized)]
  public static IJobAdapter GetJobAdapter() {
    if (jobAdapter == null) {
      jobAdapter = discoveryService.GetInstances<IJobAdapter>()[0];
    }

    return jobAdapter;
  }
}