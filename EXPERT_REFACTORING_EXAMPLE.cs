// <copyright file="EXPERT_REFACTORING_EXAMPLE.cs" company="WorkTrack Team">
// Copyright (c) WorkTrack Team. All rights reserved.
// </copyright>
//
// ПРИМЕР РЕФАКТОРИНГА ПО РЕКОМЕНДАЦИЯМ ЭКСПЕРТОВ
// Этот файл НЕ компилируется - это только пример для изучения
// Применены: SOLID, GoF Patterns, упрощение структуры
//

// См. полный пример в EXPERT_CODE_REVIEW.md
// Ключевые изменения:
// 1. Удалены избыточные методы-обертки (TryInvoke, HandleError, etc)
// 2. Выделены отдельные классы по SRP: InstallerDiscovery, InstallerFactory, InstallerRegistry
// 3. Введены интерфейсы: IInstallerDiscovery, IInstallerCreationStrategy
// 4. Применены паттерны: Factory, Strategy
// 5. Упрощен публичный API до 2 методов вместо 13
